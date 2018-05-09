# To intall in Wox
```
wpm install WebApp launcher
```

# Wox WebApp plugin

A Wox plugin to start url in a "Web app" mode.

Require Chrome installed to work out of the box.

Can be configured to work with another "web app launcher"

# Example 

## Basic usage

Enter the following command to learn wap few url to transform into webapps:

```
wap add https://maps.google.com/ directions
wap add https://youtube.com/ video google
wap add https://google.com/ search engine
wap add https://bing.com/ microsoft ms search engine
```

Now search for url:

```
wap list
```

You'll see some url:

![(wap list)](doc/01-wap-list.png)

You can select https://maps.google.com/ and it will start the url using Chrome in WebApp mode:

![(Google map in WebApp mode)](doc/02-google-map-webapp-mode.png)

## Filters

you can obviously filter on url

```
wap list google.com
```

![(wap list google.com)](doc/03-wap-list-google-com.png)

or on keywords

```
wap list video
```

![(wap list video)](doc/04-wap-list-video.png)

or both

```
wap list google
```

![(wap list google)](doc/05-wap-list-google.png)


## Quick filter

You don't need to write `list` if the query is not ambiguous. All the previous examples may have been written as:

```
wap google.com
wap video
wap google
```

# Advanced configuration

If you want to use something else to start your urls as a webapp, you can configure the plugin using `wap config`.

Imagine you have a file called mylauncher.exe that can start webapps given an url as argument. You can then type

```
wap config mylauncher.exe "{0}"
```
![(wap config mylauncher.exe "{0}")](doc/06-wap-config-mylauncher.png)

You can see your current config by simply validating `wap config`, it will auto-complete with current configuration.

Default configuration is :

```
wap config chrome.exe --app="{0}"
```


