# Wox.WebApp plugin

```
wap
-> wap [list] [PATTERNS] [PATTERNS] [...]
-> wap config [APP_PATH] [APP_PATTERN]
-> wap add URL [KEYWORDS] [KEYWORDS] [...]
-> wap remove URL
```

```
wap onf
-> wap config [APP_PATH] [APP_PATTERN]
```

```
wap config => wap config "C:\....\Chrome.exe" "{0} -app={1}"
```

```
wap config aaa => wap config "aaa" "{0} -app={1}"
wap config aaa bbb => wap config "aaa" "bbb"
```

```
wap config "aaa" "bbb" => ""
```


