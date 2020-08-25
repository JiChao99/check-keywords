# check-all-program-language-keywords

check all program language keywords, savewords

## Use

### Powershell

```powershell
 Invoke-RestMethod api.jichao.top/check/{word}
```

OR

```ps
(curl api.jichao.top/check/{word}).Content
```

### Bash

```batch
curl api.jichao.top/check/{word}
```

## TODO

- [ ] support swagger
- [ ] use System.Text.Json not Newtonsoft.Json
- [ ] react
- [ ] **classlibray use json file**

![img](checkKeywords.png)
