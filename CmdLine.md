# Important rules
* NO SPACES between switch and parameter
> ```twitterdump.exe -c config.json``` **(Wrong)**\
> ```twitterdump.exe -cconfig.json``` **(Correct)**
* All switch accepts both '-' and '/' as prefix
> ```twitterdump.exe /gc``` **(Correct)**\
> ```twitterdump.exe -gc``` **(Correct)**

# Specifying config (-c)
* ```twitterdump.exe -c<CONFIG_FILE>``` to load config file named CONFIG_FILE.

# Generate default config (-gc)
> This switch will SHUT DOWN the program after config generation.
* ```twitterdump.exe -gc``` to generate default config file 'twitterdump.json'.
* ```twitterdump.exe -gc -c<CONFIG_FILE>``` to generate default config file named CONFIG_FILE.

# Specify log file (-l)
* ```twitterdump.exe -l<LOG_FILE>``` to log on file named LOG_FILE.