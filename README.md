# TwitterDump: Simple [Aria2](https://github.com/aria2/aria2) wrapper for [Gallery-DL](https://github.com/mikf/gallery-dl)
[![Build status](https://ci.appveyor.com/api/projects/status/gh5po08wfgk6n1ck/branch/master?svg=true)](https://ci.appveyor.com/project/hsheric0210/twitterdump/branch/master)

* All sites [which supported by Gallery-DL](https://github.com/mikf/gallery-dl/blob/master/docs/supportedsites.md) are supported if you use gallery-dl as retriever
  * But you need to modify ```Parameters``` key in ```Retriever``` section* in configuration.

* **You should download Gallery-DL and Aria2 from their websites**
  * If you have existing one, you can specify its path to the config file

* Note that **you should execute the program one or more time to generate the configuration file**.
  * Also, you can generate the default config by command-line switch anytimes.

* Required resources:
  * Your PC with Windows operating system
  * Query list file (Default: list.txt)
  * [Gallery-DL](https://github.com/mikf/gallery-dl/releases), [Aria2](https://github.com/aria2/aria2/releases) executable (You should download them from their official websites)