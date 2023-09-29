# NPGR003-23
Support for ***NPGR003 (Introduction to Computer Graphics)*** lecture.
Year 2023/24.

See [NPGR003 lecture - current info](https://cgg.mff.cuni.cz/~pepca/lectures/npgr003.current.cz.php).

## Lab plan
Every `NN-*` directory refers to an item in the lab plan
(usually you will find a programming task there). You don't have
to do all of them to pass the lab, 1/2 to 2/3 is enough (see the
points in task definitions and in the shared table).

This global `README.md` defines general rules and conditions,
see individual directories for task-specific information.

## Point table
See [this shared table](https://docs.google.com/spreadsheets/d/1QLukOcSRPa5exOYW1eUfQWY2WoMjo1menbjQIU7Gvs4/edit?usp=sharing)
for current points. Please check the due dates of each task.
The primary source of organizational information is **the shared table**, not
the GIT repository.

Contact me <pepca@cgg.mff.cuni.cz> with any suggestions, comments or
complaints. If you are in a different lab group, please contact your
lab supervisor first.

## C# source files in task directories
Some directories contain support files from the teachers. The default
version uses `.NET 7`, you can change it to `.NET 6` variant
if necessary. But today (October 2023) `.NET 7` is reported to work
well on Windows, Linux and macOS.

We use `Visual Studio 2022`, the [Community](https://visualstudio.microsoft.com/vs/community/)
(free) version is good enough for all tasks.

## AI assistants
The use of **AI assistants (based on Large Language Models)** is not prohibited,
on the contrary! We welcome you to use them under two conditions:
1. you must **acknowledge** for each task that the AI assistant significantly
   helped you.
2. you must **document your use of the assistant**. For example, if you use
   ChatGPT, record the entire conversation and post a link to it.
   For more "built-in" assistants, you should write a verbal report of
   what the help looked like, how often (and how hard) you had to
   correct the machine-generated code - and if you used comments in
   the code, leave them in!

Credit bonuses/maluses for using an AI assistant are still undecided.

Over the course of the semester, some of you will have the opportunity
to write short reports and present your experiences to the rest of the
group (and you'll get extra credit for doing so).

## Notes
* If anything doesn't work well in your **Linux/macOS environment**,
  you should write me (<pepca@cgg.mff.cuni.cz>) as soon as possible.
  Of course you could report positive experience in Linux/macOS as well.
* We have a recommendation for you - use
  the `git fork` command at the beginning of the semester and
  you can work in the same directory structure. You will own your
  repository ([GitHub](https://github.com/) or
  [GitLab](https://gitlab.mff.cuni.cz/) are recommended platforms),
  you must send us the URL! Don't try to "Push"/"Pull request"
  the original repository, you won't have write permissions to it!
* Your repositories should remain **private**, you have to grant
  R/O permission to your lab supervisor:
  * Josef Pelikán - [GitHub link](https://github.com/pepcape),
	[GitLab link](https://gitlab.mff.cuni.cz/pelikan)
  * Tomáš Iser - [GitHub link](),
	[GitLab link]()
* If you want use any third party library, do it correctly and use the
  [NuGet system](https://www.nuget.org/). Many pilot projects are
  using libraries already (e.g. [SixLabors.ImageSharp](https://www.nuget.org/packages/SixLabors.ImageSharp)),
  so learn by example.
* If you need to write some text, documentation (mandatory or voluntary)
  use the [MarkDown (.md)](https://docs.github.com/en/get-started/writing-on-github/getting-started-with-writing-and-formatting-on-github/basic-writing-and-formatting-syntax)
  syntax ([another MarkDown page](https://www.markdownguide.org/basic-syntax/))

* Visual Studio 2022 supports direct **MarkDown editing** (with live
  result preview) starting from the 17.5 update

# Your Documentation
Use the [separate file DOC.md](DOC.md) in each task directory to avoid
merge conflicts. Don't edit existing `README.md` files!
