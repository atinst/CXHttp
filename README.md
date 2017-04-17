CXHttp
===

A Simple HTTP Client for C# (UWP) inspired by [Jsoup](https://jsoup.org/).

## Examples

### Simple GET

``` C#

var res = await CXHttp.Connect("http://www.baidu.com").Get();
var html = await res.Content();

```

### POST with Headers and Form Fields

``` C#

var res = await CXHttp.Connect("http://gw.bnu.edu.cn:803/srun_portal_pc.php?ac_id=1")
			.Header("User-Agent", "IE")
			.Data("action", "login")
			.Data("ac_id", "1")
			.Data("username", "username")
			.Data("password", "password")
			.Post();
var html = await res.Content();

```

### HTTP Session (Cookies persisted)

``` C#

var USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
var URL = "http://cas.bnu.edu.cn/cas/login?service=http%3A%2F%2Fzyfw.bnu.edu.cn%2FMainFrm.html";

// HTTP Get to Set-Cookie
var res = await CXHttp.Session("zyfw").req
    .Url(URL)
    .Header("User-Agent", USER_AGENT)
    .Get();

// ...
            
// One name, one session. So Cookies are shared.
res = await CXHttp.Session("zyfw").req
	.Url(URL)
	.Header("User-Agent", USER_AGENT)
	.Data("username", "username")
	.Data("password", "password")
	// ...
	.Post();

// Specific encoding
html = await res.Content("GBK");

```