namespace _03_SFC;

public interface IPredictor
{
  /// <summary>
  /// Initialize the input stream.
  /// </summary>
  void Init();

  /// <summary>
  /// Puts the next symbol.
  /// </summary>
  /// <param name="symbol">Symbol code.</param>
  void Put(int symbol);

  /// <summary>
  /// Prediction of the next symbol.
  /// </summary>
  /// <returns>Code of the predicted symbol.</returns>
  int Predict();
}

public class LinearPredictor1 : IPredictor
{
  /// <summary>
  /// Last encoded symbol. s(i-1)
  /// </summary>
  protected int last1;

  /// <summary>
  /// Initialize the input stream.
  /// </summary>
  public virtual void Init()
  {
    last1 = 0;
  }

  /// <summary>
  /// Puts the next symbol.
  /// </summary>
  /// <param name="symbol">Symbol code.</param>
  public virtual void Put(int symbol)
  {
    last1 = symbol;
  }

  /// <summary>
  /// Prediction of the next symbol.
  /// </summary>
  /// <returns>Code of the predicted symbol.</returns>
  public virtual int Predict()
  {
    return last1;
  }
}

public class PredictiveEncoder
{
  protected IPredictor Predictor;

  protected EntropyCalculator Calculator;

  public PredictiveEncoder(IPredictor predictor, EntropyCalculator calculator)
  {
    Predictor = predictor;
    Calculator = calculator;
  }

  public void Init()
  {
    Predictor?.Init();
  }

  public void Put(int symbol)
  {
    int prediction = Predictor.Predict();
    int residuum = symbol - prediction;
    Predictor.Put(symbol);

    // Process the residuum: update data for entropy computation...
    Calculator.Put(residuum);
  }

  /// <summary>
  /// Compute entropy of the current state of the stream.
  /// </summary>
  /// <returns>Entropy in bits.</returns>
  public long Entropy()
  {
    return Calculator.Entropy();
  }
}

public class EntropyCalculator
{
  /// <summary>
  /// Zig-zag encoding is used to process both positive and negative symbol codes...
  /// </summary>
  protected List<long> Histogram = new();

  /// <summary>
  /// Total number of input samples.
  /// </summary>
  public long Total = 0;

  /// <summary>
  /// Reset the calculator.
  /// </summary>
  public void Init()
  {
    Histogram = new();
    Total = 0;
  }

  /// <summary>
  /// Put next symbol.
  /// </summary>
  /// <param name="value">Symbol code (values around zero are preferred).</param>
  public void Put(int value)
  {
    int index = (int)ZigZagEncode(value);
    while (index >= Histogram.Count)
      Histogram.Add(0);
    Histogram[index]++;
    Total++;
  }

  /// <summary>
  /// Total entropy of the message in bits.
  /// Divide this number by "Total" for the average sample entropy.
  /// </summary>
  /// <returns>Entropy in bits.</returns>
  public long Entropy()
  {
    double entropy = 0.0;
    foreach (long item in Histogram)
    {
      if (item > 0)
      {
        double count = (double)item;
        double probability = count / Total;
        entropy -= probability * Math.Log(probability, 2);
      }
    }
    return (long)Math.Round(entropy * Total);
  }

  public static uint ZigZagEncode(int value)
  {
    return (uint)((value << 1) ^ (value >> 31));
  }

  public static int ZigZagDecode(uint value)
  {
    return (int)((value >> 1) ^ (-(value & 1)));
  }
}
