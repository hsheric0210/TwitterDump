# TwitterDump: Download images using [Gallery-DL](https://github.com/mikf/gallery-dl) + [Aria2](https://github.com/aria2/aria2)
[![Build status](https://ci.appveyor.com/api/projects/status/gh5po08wfgk6n1ck/branch/master?svg=true)](https://ci.appveyor.com/project/hsheric0210/twitterdump/branch/master)

* All sites [which supported by Gallery-DL](https://github.com/mikf/gallery-dl/blob/master/docs/supportedsites.md) are supported if you use gallery-dl as retriever
  * But you need to modify ```Parameters``` key in ```Retriever``` section* in configuration.

* **You should download Gallery-DL and Aria2 from their websites**
  * Or, you can specify its location in <abbr title="TwitterDump.json, by default">configuration file</abbr>

* Note that **you should execute the program one or more time to generate the configuration file**.
  * Or, you can generate the default config by command-line switch anytimes.

* Needed resource file name:
  * Query file (Default: list.txt)
  * Destination folder (Default: Downloaded\{Query})**
