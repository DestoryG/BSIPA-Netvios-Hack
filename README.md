# Reverse engineering the Chinese version of Beat Saber

Ah, Beat Saber, since its release in 2018, has consistently been one of the most popular VR games.

But did you know that Beat Games once collaborated with a Chinese game developer to release a "Chinese version of Beat Saber"?

So, there it is.

<img width="1919" height="991" alt="CNBeatSaber" src="https://gist.github.com/user-attachments/assets/304b6c37-dff7-44e4-be83-870c62e8cbaa" />

This version of Beat Saber called "节奏空间(Beat Space ?)". It's a product of a collaboration between Beat Games and the Chinese game company Netease, whose representative games include Identity V (very popular in China and Japan), Marvel Rivals, and Naraka Bladepoint. But the last update seems to have been in 2022? Well, at least we might get a new UI.

<img width="1230" height="786" alt="QQ20260205-102831" src="https://gist.github.com/user-attachments/assets/9cceac83-4600-4e1a-9fc3-b7c675f31180" />

Anyway, we need to download this and research it carefully, but, this is a real shock right from the start: We can't download the game directly; it seems we have to download a game platform called "Netvios"? Let's see.

<img width="1919" height="944" alt="eff11c14-e65f-4f2f-9060-6bbb963b1743" src="https://gist.github.com/user-attachments/assets/25eacee4-5707-4381-adf3-02527d26598d" />

After I registered an account using my mobile phone number (this is the only way to register), this thing seemed to take me to a "store registration" page. Oh, so this is for those game arcade room with VR? However, they did say that if you are a personal user, you only need to fill in "personal" in the store name.

<img width="1919" height="1032" alt="image" src="https://gist.github.com/user-attachments/assets/c44e1039-b831-4cce-9571-ffe83ff20699" />

So after doing all that, I can finally download this mysterious Beat Saber version!

<img width="1919" height="1019" alt="image" src="https://gist.github.com/user-attachments/assets/4e24d161-f23a-447e-be90-b3680feab74a" />

Before we start the game, let’s take a look at the game itself. Well, at first glance, it’s just a Beat Saber with BSIPA and some plugins? You know, BSIPA uses the MIT license, which doesn't restrict you from redistributing it as a binary file or using it for commercial purposes, right? and a DLL file called NetviosSDK that looks like DRM.

<img width="393" height="679" alt="image" src="https://gist.github.com/user-attachments/assets/a26f597c-1a53-4d11-b699-762c8774345d" />

Before that, I wanted to check if they truly respected the MIT license. It's simple enough, just put a MIT license here, and I quickly found the MIT license files for all the MIT modules in Plugins\licenses, but... BSIPA's weren't there. I looked everywhere, but I couldn't find it, which basically constitutes a violation of MIT. Um... okay? After all, they put the MIT licenses for all the other modules there, so at least we can assume they forgot.

<img width="253" height="364" alt="QQ20260205-111230" src="https://gist.github.com/user-attachments/assets/0689e84a-31c7-42af-bdb8-2f81c577f570" />

And now we should play it!

First, ignoring the extremely... well, "nostalgic" UI, I noticed a timer HUD appearing on my computer screen.

<img width="1919" height="1079" alt="image" src="https://gist.github.com/user-attachments/assets/0c5de88a-ba6f-4ded-983a-f5fb0f487800" />

After checking again, I discovered that they use a pay-by-the-hour system for the Chinese version of Beat Saber, charging users with points, which further confirms its suitability for arcade room. According to the pricing , 20 points allow for 10 minutes of play, and 10 points can be purchased for every 1 CNY (0.14 USD).

Let's ignore all that and actually play a game. Well, it turns out they used Beat Saber version 1.10.0, which was released in 2020 and is still using the old interface.

Then, we have a "More Songs" button to download custom levels! But... what are these? And what is beatsaberbbs? Where's my BeatSaver? It looks like they've modified BeatSaverDownloader.

<img width="1920" height="1080" alt="image" src="https://gist.github.com/user-attachments/assets/be954d89-9acd-4b35-98c4-73817996b8ca" />

So, is it possible for us to delete this timer thing? First, let's uninstall Netvios and then try opening the game.
The game launched, but it started counting down and popped up a message: "Hey kid, where's Netvios? Find him for me!" The game crashed after the countdown ended.

<img width="1920" height="1080" alt="image" src="https://gist.github.com/user-attachments/assets/8e5bc807-0539-4605-a17f-3e179fa3de9e" />

So all we need to do is delete NetviosSDK.dll, right? No, it still crashes, but this time the game doesn't even run, whereas it did run briefly before.
I started looking for the cause, and soon I found something suspicious in the log:

    [ERROR @ 23:35:57 | NetviosSdkPlugin] NetviosSDK_Init error: System.DllNotFoundException: NetViosSDK
    [ERROR @ 23:35:57 | NetviosSdkPlugin]   at (wrapper managed-to-native) NetviosSdkPlugin.NetviosSdkPluginController.NetViosSDK_Init(string,string,NetviosSdkPlugin.NetviosSdkPluginController/InitCallback)
    [ERROR @ 23:35:57 | NetviosSdkPlugin]   at NetviosSdkPlugin.NetviosSdkPluginController.SetupInHealthWarningScene () [0x00019] in <3715bab568964081b1cf1782acef1067>:0 
    [CRITICAL @ 23:35:57 | UnityEngine] DllNotFoundException: NetViosSDK
    [CRITICAL @ 23:35:57 | UnityEngine] NetviosSdkPlugin.NetviosSdkPluginController.OnApplicationQuit () (at <3715bab568964081b1cf1782acef1067>:0)

A mod called NetviosSdkPlugin executed OnApplicationQuit directly because it couldn't find NetviosSDK.dll, Let's just delete it and see.

    [WARNING @ 20:54:13 | IPA/Injector] No backup found! Was BSIPA installed using the installer?
    [WARNING @ 20:54:13 | IPA/LibraryLoader] Could not add DLL directory 
    [WARNING @ 20:54:13 | IPA/LibraryLoader] System.ComponentModel.Win32Exception (0x80004005): 参数错误。
    [WARNING @ 20:54:14 | IPA/Loader] Bare manifest BeatSaverSharp.manifest does not declare any files. Dependency resolution and verification cannot be completed.
    [ERROR @ 20:54:15 | IPA/Loader] sdk plugin or playerData plugin not included plugin dir
    
I didn't even open the game, but the last log is suspicious. I don't think the BSMG would write sentences in all lowercase in their logs.
 
 I could actually just drag this into dnspy ​​to decompile it, but I think as a large game company, Netease must have obfuscated it, so I decided to write a small plugin to block NetviosSdkPlugin to quit the game.

<img width="1920" height="1040" alt="image" src="https://gist.github.com/user-attachments/assets/affe5455-bc00-4deb-9674-69c6b57cdead" />

Unfortunately, I didn't see any indication in the logs that my plugin was loaded. NetviosSdkPlugin still executed OnApplicationQuit, but this time the logs still contained suspicious information.


    [WARNING @ 23:19:46 | IPA/Injector] No backup found! Was BSIPA installed using the installer?
    [WARNING @ 23:19:46 | IPA/LibraryLoader] Could not add DLL directory 
    [WARNING @ 23:19:46 | IPA/LibraryLoader] System.ComponentModel.Win32Exception (0x80004005): 参数错误。
    [WARNING @ 23:19:47 | IPA/Loader] Bare manifest BeatSaverSharp.manifest does not declare any files. Dependency resolution and verification cannot be completed.
    [WARNING @ 23:19:47 | IPA/Loader] FuckNetvios Unauthorized
   
What do you mean "FuckNetvios Unauthorized" ? I don't recall BSIPA ever having this warning. Now I'll try dragging a confirmed working mod into the Plugins folder.

    [WARNING @ 23:33:46 | IPA/Injector] No backup found! Was BSIPA installed using the installer?
    [WARNING @ 23:33:47 | IPA/LibraryLoader] Could not add DLL directory 
    [WARNING @ 23:33:47 | IPA/LibraryLoader] System.ComponentModel.Win32Exception (0x80004005): 参数错误。
    [ERROR @ 23:33:47 | IPA/Loader] No plugin found in the manifest namespace (FuckNetvios) in NetviosHelperPlugin.dll
    [WARNING @ 23:33:47 | IPA/Loader] Bare manifest BeatSaverSharp.manifest does not declare any files. Dependency resolution and verification cannot be completed.
    [WARNING @ 23:33:49 | IPA/Loader] FireworksDisabler Unauthorized
    [WARNING @ 23:33:49 | UnityEngine] OnLevelWasLoaded was found on PluginComponent

Wow, it still says "Unauthorized". This means Netease modified BSIPA and added it to the whitelist.

Let's take a look at the main program of BSIPA. The version is actually 4.0.6? This is interesting. The BSIPA GitHub repository shows 4.0.5, then jumps directly to 4.1.0; 4.0.6 doesn't exist at all. Clearly, even BSIPA has been modified.

<img width="470" height="651" alt="image" src="https://gist.github.com/user-attachments/assets/3d35aad1-e45d-4974-a444-60208427226e" />

Now, Try a "Trojan horse" approach, changing an existing mod to our own! The seemingly inactive NetviosHelperPlugin is the perfect target!

    [WARNING @ 23:41:38 | IPA/Injector] No backup found! Was BSIPA installed using the installer?
    [WARNING @ 23:41:38 | IPA/LibraryLoader] Could not add DLL directory 
    [WARNING @ 23:41:38 | IPA/LibraryLoader] System.ComponentModel.Win32Exception (0x80004005): 参数错误。
    [WARNING @ 23:41:39 | IPA/Loader] Bare manifest BeatSaverSharp.manifest does not declare any files. Dependency resolution and verification cannot be completed.
    [WARNING @ 23:41:40 | IPA/Loader] FireworksDisabler Unauthorized
    [WARNING @ 23:41:40 | IPA/Loader] NetviosHelperPlugin Unauthorized
    [WARNING @ 23:41:40 | UnityEngine] OnLevelWasLoaded was found on PluginComponent

Oops, it seems this whitelist isn't based on filenames; perhaps there's other code checking availability? Even so, it's time to try dnSpy?

<img width="1920" height="1040" alt="image" src="https://gist.github.com/user-attachments/assets/7085ebfa-d62f-496e-baa4-9ef202012745" />

Emmm, Gosh, I really didn't expect that they didn't do any obfuscation to prevent dnspy. So, since the problem is with SetupInHealthWarningScene, we just need to change that, right? This also further ensures the BSIPA whitelist is in place.

<img width="1144" height="484" alt="image" src="https://gist.github.com/user-attachments/assets/5486bbec-ef88-4967-9a3a-5ab9a5ff743b" />

    [WARNING @ 00:01:13 | IPA/Injector] No backup found! Was BSIPA installed using the installer?
    [WARNING @ 00:01:14 | IPA/LibraryLoader] Could not add DLL directory 
    [WARNING @ 00:01:14 | IPA/LibraryLoader] System.ComponentModel.Win32Exception (0x80004005): 参数错误。
    [WARNING @ 00:01:15 | IPA/Loader] Bare manifest BeatSaverSharp.manifest does not declare any files. Dependency resolution and verification cannot be completed.
    [WARNING @ 00:01:15 | IPA/Loader] NetviosSdkPlugin Unauthorized
    [ERROR @ 00:01:15 | IPA/Loader] sdk plugin or playerData plugin not included plugin dir

No, it appears they're using hash value verification for the whitelist. In that case, we should check IPA.Loader.dll.

But before we check, we neet to replaced the game with the official IPA 4.0.5 from Github for check. Which worked, but the problem was that we lost all the Chinese localization modifications. It seemed to revert to the original official Beat Saber, which is not what we wanted.

<img width="1919" height="1079" alt="image" src="https://gist.github.com/user-attachments/assets/1fbc2827-dad9-4ae5-bb43-9dcfc01ff9e3" />

Oh, Their modified BSIPA added the IPA.Netvios class that did not exist originally. Let's check some main code.

<img width="532" height="581" alt="image" src="https://gist.github.com/user-attachments/assets/d67cb538-84b0-431f-86ed-6e71f31e1ab7" />

 - IPA.Netvios.Config
```
    using  System;  
      
    namespace  IPA.Netvios  
    {  
    // Token: 0x02000028 RID: 40  
    internal  class  Config  
    {  
    // Token: 0x060000E2 RID: 226 RVA: 0x0000459B File Offset: 0x0000279B  
    internal  static  string  GetModHost()  
    {  
    return  "https://beatsaberbbs.com";  
    }  
      
    // Token: 0x060000E3 RID: 227 RVA: 0x000045A2 File Offset: 0x000027A2  
    internal  static  string  GetModApiUrlPrefix()  
    {  
    return  Config.GetModHost()  +  "/api/mod/v1/ipa/plugins";  
    }  
      
    // Token: 0x0400003C RID: 60  
    private  const  string  _devModHost  =  "http://dev.beatsaberbbs.com";  
      
    // Token: 0x0400003D RID: 61  
    private  const  string  _pubModHost  =  "https://beatsaberbbs.com";  
    }  
    }
```
This is the configuration file for the entire Netvios system. It hardcodes that `GetModHost()` returns "https://beatsaberbbs.com", and retrieves the plugin whitelist via https://beatsaberbbs.com/api/mod/v1/ipa/plugins. After accessing it, I found it's in JSON format：
```
[{"id":"BSIPA","key":"944ff99feea2122246ed10712a2df8d9","name":"Base","version":"4.0.6"},
{"id":"BS_Utils","key":"27b3f3f9b14d5b7a8c51bf3e2e00e818","name":"BS_Utils","version":"1.4.9"},
{"id":"BeatSaberMarkupLanguage","key":"e2d702a9f1774764d6819840af4092bf","name":"BeatSaberMarkupLanguage","version":"1.3.2"},
{"id":"CustomAvatars","key":"630f8956c2e87b9bd248fb65bd0d3d7e","name":"Custom Avatars","version":"5.0.0-beta.7"},
{"id":"CameraPlus","key":"df5470c706a8a73290ecdb279e2529eb","name":"CameraPlus","version":"4.2.0"},
{"id":"DynamicOpenVR.BeatSaber","key":"c9adac1aa136de7615eea8e94325557f","name":"DynamicOpenVR.BeatSaber","version":"0.2.1"},
{"id":"SongCore","key":"d4ed975d112c04f2769598befa5b8249","name":"SongCore","version":"2.9.6"},
{"id":"bs_debug","key":"97586d985fac787e4dcf1e1df350e126","name":"bs_debug","version":"0.0.1"},
{"id":"BSPluginTest","key":"c37c84ac16a59c20feae52554019f0b7","name":"BSPluginTest","version":"0.0.0"},
{"id":"NetviosSdkPlugin","key":"396508d9763c7689d6cd56bbf8237382","name":"NetviosSdkPlugin","version":"0.0.1"},
{"id":"CloudSdkPlugin","key":"ac0c01c4b4e307ff84a52aa69ce2c441","name":"CloudSdkPlugin","version":"0.0.1"},
{"id":"LocalizationPlugin","key":"bbeced69f322a4ca9290e605fcce8f17","name":"LocalizationPlugin","version":"0.0.1"},
{"id":"NetviosHelperPlugin","key":"51a7847e13d99016e9cd825d80dac110","name":"NetviosHelperPlugin","version":"0.0.1"},
{"id":"PlayerDataPlugin","key":"bd55fce3fcf4d7d3a74866800ef3fc2f","name":"PlayerDataPlugin","version":"0.0.1"},
{"id":"BeatSaverSharp","key":"2f1a153b23681d7246ec495271396976","name":"BeatSaverSharp","version":"1.5.2"},
{"id":"BeatSaverDownloader","key":"777c7decf6c9c30a594e8afb93dcf6d6","name":"BeatSaverDownloader","version":"5.1.2"},
{"id":"bs_debug","key":"3fbab9b5604c96db38c4359406cd76fa","name":"bs_debug","version":"0.0.2"},
{"id":"CommonPlugin","key":"a33b127e813f7fb944414b5c207efc0d","name":"CommonPlugin","version":"0.0.1"},
{"id":"DynamicOpenVR","key":"4f9d68354741a44933d0a6e19fb106e3","name":"DynamicOpenVR","version":"0.2.1"},
{"id":"BeatSaverSharp","key":"74aa5040890ee393ff52dc65abd2b1f4","name":"BeatSaverSharp","version":"1.5.3"},
{"id":"DynamicOpenVR.BeatSaber","key":"7a130fda44b3c8ed8570500889e7165b","name":"DynamicOpenVR.BeatSaber","version":"0.2.3"},
{"id":"NetviosHelperPlugin","key":"b30e8ccc55ae38d61993bc8e4b5c40d5","name":"NetviosHelperPlugin","version":"0.0.2"},
{"id":"SongCore","key":"3de259eea593be551d21ac0493cbe30f","name":"SongCore","version":"2.9.7"},
{"id":"Custom Sabers","key":"4f1d6afb0d8cd8a95bc909af8d119fbb","name":"Custom Sabers","version":"4.4.0"},
{"id":"SongCore","key":"1dd3279c4bdd4876019bf741b2a7e2f9","name":"SongCore","version":"2.9.8"},
{"id":"NetviosHelperPlugin","key":"d80a5254dab6dec34630b6478fb66a23","name":"NetviosHelperPlugin","version":"0.0.3"},
{"id":"SongCore","key":"323333cb13601ff0751cbb4b864cae1b","name":"SongCore","version":"2.9.9"},
{"id":"PlayerDataPlugin","key":"369b285dddebf908a196f582dbd7b7ba","name":"PlayerDataPlugin","version":"0.0.2"},
{"id":"PlayerDataPlugin","key":"882686b92ca9394a0132cbc5b93ea79f","name":"PlayerDataPlugin","version":"0.0.3"},
{"id":"NetviosHelperPlugin","key":"b82b87a5c940cd0593aef172b1ce6dd2","name":"NetviosHelperPlugin","version":"0.0.4"},
{"id":"BeatSaberMultiplayer","key":"efba083397b1be7a427b79d0565e70bd","name":"BeatSaberMultiplayer","version":"0.0.1"},
{"id":"PlayerDataPlugin","key":"6028da93cfaf4d83233d3879fa00ba76","name":"PlayerDataPlugin","version":"0.0.4"},
{"id":"NalulunaModifier","key":"8034c0ff734ec08306ad85671f0a3b6c","name":"NalulunaModifier","version":"0.0.12"},
{"id":"SongCore","key":"3ed2a4c0129e1d0f0aaf564adc96eded","name":"SongCore","version":"2.9.10"},
{"id":"BeatSaverDownloader","key":"b6ff8140dae2904dadfdb49dada21ec3","name":"BeatSaverDownloader","version":"5.1.3"},
{"id":"BeatSaberMultiplayer","key":"93aa7e98fb2f48f76dafaa23d58429e0","name":"BeatSaberMultiplayer","version":"0.1.0"},
{"id":"BeatSaverDownloader","key":"e5fe4bdf85a94b9ff7406aa269fd1039","name":"BeatSaverDownloader","version":"5.1.4"},
{"id":"NetviosSdkPlugin","key":"92a37a9f4b3ae9f3f3221b945e0f1bdd","name":"NetviosSdkPlugin","version":"1.0.0"},
{"id":"BeatSaberMultiplayer","key":"13dc1ad821204a45ba02dffff68ca46c","name":"BeatSaberMultiplayer","version":"0.2.0"},
{"id":"BeatSaberMultiplayer","key":"88d92c9df31db8a9fe51b5c4866e1f99","name":"BeatSaberMultiplayer","version":"0.3.0"},
{"id":"NetviosHelperPlugin","key":"cd48eace2bb156996cda7cff5e34e646","name":"NetviosHelperPlugin","version":"0.0.5"},
{"id":"BeatSaberMultiplayer","key":"ba7e84d224f53d45994ce50acd3a71be","name":"BeatSaberMultiplayer","version":"0.4.0"},
{"id":"BeatSaberMultiplayer","key":"27b19ddf93fa8a8bfacae8e5277087e0","name":"BeatSaberMultiplayer","version":"0.5.0"},
{"id":"BeatSaberMultiplayer","key":"800590220e44547176cf011c67582a8f","name":"BeatSaberMultiplayer","version":"0.6.0"}]
```

This is bad, meaning if this website goes down, or they close the API, the game will be completely unusable.
 - IPA.Netvios.Utils
```
using  System;  
using  System.IO;  
using  System.Security.Cryptography;  
using  System.Text;  
  
namespace  IPA.Netvios  
{  
///  <summary>  
/// 公共功能类  
///  </summary>  
// Token: 0x0200002E RID: 46  
public  class  Utils  
{  
///  <summary>  
/// 获取文件MD5值  
///  </summary>  
///  <param name="fileName">文件绝对路径</param>  
///  <returns></returns>  
// Token: 0x06000100 RID: 256 RVA: 0x00004BE4 File Offset: 0x00002DE4  
public  static  string  GetMD5HashFromFile(string  fileName)  
{  
string  text;  
try  
{  
FileStream  file  =  new  FileStream(fileName,  FileMode.Open);  
byte[]  retVal  =  new  MD5CryptoServiceProvider().ComputeHash(file);  
file.Close();  
StringBuilder  sb  =  new  StringBuilder();  
for  (int  i  =  0;  i  <  retVal.Length;  i++)  
{  
sb.Append(retVal[i].ToString("x2"));  
}  
text  =  sb.ToString();  
}  
catch  (Exception  ex)  
{  
throw  new  Exception("GetMD5HashFromFile() fail,error:"  +  ex.Message);  
}  
return  text;  
}  
  
///  <summary>  
/// check me  
///  </summary>  
///  <returns></returns>  
// Token: 0x06000101 RID: 257 RVA: 0x00004C6C File Offset: 0x00002E6C  
public  static  bool  CheckIPA()  
{  
return  true;  
}  
}  
}
```
This is a utility class for verifying files.

MD5 Calculator (GetMD5HashFromFile): This function calculates the MD5 hash value of every DLL file in your local Plugins folder.

Purpose: The calculated hash value is compared with a "whitelist" issued by the server. If the hash value of your file (such as a modified DLL) does not match, the previously mentioned Unauthorized error will be triggered.

CheckIPA(): This method currently only returns true, and appears to be an incomplete or deprecated self-verification function, possibly originally intended to check whether the BSIPA itself has been tampered with.

 - IPA.Netvios.HttpClientHelper

```
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace IPA.Netvios
{
	/// <summary>
	/// 网络请求类
	/// </summary>
	// Token: 0x02000029 RID: 41
	public class HttpClientHelper
	{
		/// <summary>
		/// Get方法获取Json数据
		/// </summary>
		/// <param name="url"></param>
		/// <param name="webProxy"></param>
		/// <returns></returns>
		// Token: 0x060000E5 RID: 229 RVA: 0x000045BC File Offset: 0x000027BC
		public static string GetHttpResponseJson(string url, IWebProxy webProxy)
		{
			string empty = string.Empty;
			Encoding encoding = Encoding.UTF8;
			return HttpClientHelper.GetStream(HttpClientHelper.CreateGetHttpResponse(new HttpGetParametersModel
			{
				Url = url,
				WebProxy = webProxy,
				Timeout = new int?(20000)
			}), encoding);
		}

		/// <summary>
		/// Post Url获取Json数据
		/// </summary>
		/// <param name="url"></param>
		/// <param name="webProxy"></param>
		/// <returns></returns>
		// Token: 0x060000E6 RID: 230 RVA: 0x00004604 File Offset: 0x00002804
		public static string PostHttpResponseJson(string url, IWebProxy webProxy)
		{
			string empty = string.Empty;
			Encoding encoding = Encoding.UTF8;
			return HttpClientHelper.GetStream(HttpClientHelper.CreatePostHttpResponse(new HttpPostParametersModel
			{
				Url = url,
				RequestEncoding = encoding,
				WebProxy = webProxy,
				Timeout = new int?(20000)
			}), encoding);
		}

		/// <summary>
		///  Post带参数的 Url获取Json数据
		/// </summary>
		/// <param name="url"></param>
		/// <param name="webProxy"></param>
		/// <param name="postDict"></param>
		/// <returns></returns>
		// Token: 0x060000E7 RID: 231 RVA: 0x00004654 File Offset: 0x00002854
		public static string PostHttpResponseJson(string url, IWebProxy webProxy, IDictionary<string, string> postDict)
		{
			string empty = string.Empty;
			Encoding encoding = Encoding.UTF8;
			return HttpClientHelper.GetStream(HttpClientHelper.CreatePostHttpResponse(new HttpPostParametersModel
			{
				Url = url,
				DictParameters = postDict,
				WebProxy = webProxy,
				RequestEncoding = encoding,
				Timeout = new int?(20000)
			}), encoding);
		}

		/// <summary>
		/// 创建GET方式的HTTP请求
		/// </summary>
		/// <returns></returns>
		// Token: 0x060000E8 RID: 232 RVA: 0x000046AC File Offset: 0x000028AC
		public static HttpWebResponse CreateGetHttpResponse(HttpGetParametersModel getParametersModel)
		{
			if (string.IsNullOrEmpty(getParametersModel.Url))
			{
				throw new ArgumentNullException("getParametersModel.Url");
			}
			HttpWebRequest request = WebRequest.Create(getParametersModel.Url) as HttpWebRequest;
			if (getParametersModel.WebProxy != null)
			{
				request.Proxy = getParametersModel.WebProxy;
			}
			request.Method = "GET";
			request.UserAgent = HttpClientHelper.DefaultUserAgent;
			if (!string.IsNullOrEmpty(getParametersModel.UserAgent))
			{
				request.UserAgent = getParametersModel.UserAgent;
			}
			if (getParametersModel.Timeout != null)
			{
				request.Timeout = getParametersModel.Timeout.Value;
			}
			if (getParametersModel.Cookies != null)
			{
				request.CookieContainer = new CookieContainer();
				request.CookieContainer.Add(getParametersModel.Cookies);
			}
			return request.GetResponse() as HttpWebResponse;
		}

		/// <summary>
		/// 创建POST方式的HTTP请求
		/// </summary>
		/// <returns></returns>
		// Token: 0x060000E9 RID: 233 RVA: 0x00004778 File Offset: 0x00002978
		public static HttpWebResponse CreatePostHttpResponse(HttpPostParametersModel postParametersModel)
		{
			if (string.IsNullOrEmpty(postParametersModel.Url))
			{
				throw new ArgumentNullException("postParametersModel.Url");
			}
			if (postParametersModel.RequestEncoding == null)
			{
				throw new ArgumentNullException("postParametersModel.RequestEncoding");
			}
			HttpWebRequest request = null;
			if (postParametersModel.Url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
			{
				ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(HttpClientHelper.CheckValidationResult);
				request = WebRequest.Create(postParametersModel.Url) as HttpWebRequest;
				request.ProtocolVersion = HttpVersion.Version10;
			}
			else
			{
				request = WebRequest.Create(postParametersModel.Url) as HttpWebRequest;
			}
			if (postParametersModel.WebProxy != null)
			{
				request.Proxy = postParametersModel.WebProxy;
			}
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			if (!string.IsNullOrEmpty(postParametersModel.UserAgent))
			{
				request.UserAgent = postParametersModel.UserAgent;
			}
			else
			{
				request.UserAgent = HttpClientHelper.DefaultUserAgent;
			}
			if (postParametersModel.Timeout != null)
			{
				request.Timeout = postParametersModel.Timeout.Value;
			}
			if (postParametersModel.Cookies != null)
			{
				request.CookieContainer = new CookieContainer();
				request.CookieContainer.Add(postParametersModel.Cookies);
			}
			if (postParametersModel.DictParameters != null && postParametersModel.DictParameters.Count != 0)
			{
				StringBuilder buffer = new StringBuilder();
				int i = 0;
				foreach (string key in postParametersModel.DictParameters.Keys)
				{
					if (i > 0)
					{
						buffer.AppendFormat("&{0}={1}", key, postParametersModel.DictParameters[key]);
					}
					else
					{
						buffer.AppendFormat("{0}={1}", key, postParametersModel.DictParameters[key]);
					}
					i++;
				}
				byte[] data = postParametersModel.RequestEncoding.GetBytes(buffer.ToString());
				using (Stream stream = request.GetRequestStream())
				{
					stream.Write(data, 0, data.Length);
				}
			}
			return request.GetResponse() as HttpWebResponse;
		}

		/// <summary>
		/// 发送Post Json 请求 返回JSon数据
		/// </summary>
		/// <param name="JSONData">要处理的JSON数据</param>
		/// <param name="Url">要提交的URL</param>
		/// <returns>返回的JSON处理字符串</returns>
		// Token: 0x060000EA RID: 234 RVA: 0x00004994 File Offset: 0x00002B94
		public static string GetResponseData(string JSONData, string Url)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(JSONData);
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
			httpWebRequest.Method = "POST";
			httpWebRequest.ContentLength = (long)bytes.Length;
			httpWebRequest.ContentType = "application/json;charset=UTF-8";
			httpWebRequest.GetRequestStream().Write(bytes, 0, bytes.Length);
			httpWebRequest.Timeout = 60000;
			httpWebRequest.Headers.Set("Pragma", "no-cache");
			Stream streamReceive = ((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream();
			Encoding encoding = Encoding.UTF8;
			StreamReader streamReader = new StreamReader(streamReceive, encoding);
			string strResult = streamReader.ReadToEnd();
			streamReceive.Dispose();
			streamReader.Dispose();
			return strResult;
		}

		/// <summary>
		/// 设置https证书校验机制,默认返回True,验证通过
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="certificate"></param>
		/// <param name="chain"></param>
		/// <param name="errors"></param>
		/// <returns></returns>
		// Token: 0x060000EB RID: 235 RVA: 0x00004A38 File Offset: 0x00002C38
		private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
		{
			return true;
		}

		/// <summary>
		/// 将response转换成文本
		/// </summary>
		/// <param name="response"></param>
		/// <param name="encoding"></param>
		/// <returns></returns>
		// Token: 0x060000EC RID: 236 RVA: 0x00004A3C File Offset: 0x00002C3C
		private static string GetStream(HttpWebResponse response, Encoding encoding)
		{
			try
			{
				if (response.StatusCode == HttpStatusCode.OK)
				{
					string text = response.ContentEncoding.ToLower();
					if (text != null && text == "gzip")
					{
						string text2 = HttpClientHelper.Decompress(response.GetResponseStream(), encoding);
						response.Close();
						return text2;
					}
					using (StreamReader sr = new StreamReader(response.GetResponseStream(), encoding))
					{
						string text3 = sr.ReadToEnd();
						sr.Close();
						sr.Dispose();
						response.Close();
						return text3;
					}
				}
				response.Close();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return "";
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00004AE8 File Offset: 0x00002CE8
		private static string Decompress(Stream stream, Encoding encoding)
		{
			new byte[100];
			string text;
			using (GZipStream gz = new GZipStream(stream, CompressionMode.Decompress))
			{
				using (StreamReader reader = new StreamReader(gz, encoding))
				{
					text = reader.ReadToEnd();
				}
			}
			return text;
		}

		// Token: 0x0400003E RID: 62
		private static readonly string DefaultUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.70 Safari/537.36";
	}
}

```
This is a standard HTTP request: it encapsulates GET and POST requests to download whitelisted data, but it contains a major security vulnerability:

`private static bool CheckValidationResult(...) { return true; }`

They chose to let the client accept all SSL certificates and ignore any errors. This means that if someone performs a MITM attack on this URL, the game will accept it without question, offering absolutely no security. This is typically a backdoor left by poor developers or someone trying to save time.

Next, let's take a look at the IPA loading plugin code: IPA.Loader.PluginLoader.

<img width="521" height="750" alt="image" src="https://gist.github.com/user-attachments/assets/c136b110-043c-4a3c-b2c8-343063980829" />

Netease added at least three suspicious entries to this: 

 1. RefreshOfficePlugins 
 2. FilterUnauthorized 
 3. DecodeString

```RefreshOfficePlugins``` sends an HTTP request to the remote server to retrieve JSON data and uses the C# `dynamic` keyword to parse the JSON.

It extracts specific hash values ​​for ```CloudSdkPlugin```, ```NetviosSdkPlugin```, and ```PlayerDataPlugin``` from the server response and stores them in a whitelist.

If these plugins are not in the server's returned list, or if the list is empty, it directly calls `Application.Quit()` to forcibly close the game.
```
// IPA.Loader.PluginLoader  
// Token: 0x060001BD RID: 445 RVA: 0x00008670 File Offset: 0x00006870  
private  static  void  RefreshOfficePlugins()  
{  
string  url  =  Config.GetModApiUrlPrefix();  
string  res  =  "";  
try  
{  
res  =  HttpClientHelper.GetHttpResponseJson(url,  null);  
if  (string.IsNullOrEmpty(res))  
{  
throw  new  Exception("response content is empty");  
}  
}  
catch  (Exception  e)  
{  
IPA.Logging.Logger  loader  =  IPA.Logging.Logger.loader;  
string  text  =  "request web api error: ";  
Exception  ex  =  e;  
loader.Error(text  +  ((ex  !=  null)  ?  ex.ToString()  :  null));  
return;  
}  
object  pluginJson  =  JsonConvert.DeserializeObject<object>(res);  
if  (PluginLoader.<>o__12.<>p__28  ==  null)  
{  
PluginLoader.<>o__12.<>p__28  =  CallSite<Func<CallSite,  object,  IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None,  typeof(IEnumerable),  typeof(PluginLoader)));  
}  
foreach  (object  p  in  PluginLoader.<>o__12.<>p__28.Target(PluginLoader.<>o__12.<>p__28,  pluginJson))  
{  
string  text2  =  "{0}-{1}";  
if  (PluginLoader.<>o__12.<>p__1  ==  null)  
{  
PluginLoader.<>o__12.<>p__1  =  CallSite<Func<CallSite,  object,  object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None,  "ToString",  null,  typeof(PluginLoader),  new  CSharpArgumentInfo[]  {  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None,  null)  }));  
}  
Func<CallSite,  object,  object>  target  =  PluginLoader.<>o__12.<>p__1.Target;  
CallSite  <>p__  =  PluginLoader.<>o__12.<>p__1;  
if  (PluginLoader.<>o__12.<>p__0  ==  null)  
{  
PluginLoader.<>o__12.<>p__0  =  CallSite<Func<CallSite,  object,  object>>.Create(Binder.GetMember(CSharpBinderFlags.None,  "id",  typeof(PluginLoader),  new  CSharpArgumentInfo[]  {  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None,  null)  }));  
}  
object  obj  =  target(<>p__,  PluginLoader.<>o__12.<>p__0.Target(PluginLoader.<>o__12.<>p__0,  p));  
if  (PluginLoader.<>o__12.<>p__3  ==  null)  
{  
PluginLoader.<>o__12.<>p__3  =  CallSite<Func<CallSite,  object,  object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None,  "ToString",  null,  typeof(PluginLoader),  new  CSharpArgumentInfo[]  {  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None,  null)  }));  
}  
Func<CallSite,  object,  object>  target2  =  PluginLoader.<>o__12.<>p__3.Target;  
CallSite  <>p__2  =  PluginLoader.<>o__12.<>p__3;  
if  (PluginLoader.<>o__12.<>p__2  ==  null)  
{  
PluginLoader.<>o__12.<>p__2  =  CallSite<Func<CallSite,  object,  object>>.Create(Binder.GetMember(CSharpBinderFlags.None,  "version",  typeof(PluginLoader),  new  CSharpArgumentInfo[]  {  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None,  null)  }));  
}  
string  key  =  string.Format(text2,  obj,  target2(<>p__2,  PluginLoader.<>o__12.<>p__2.Target(PluginLoader.<>o__12.<>p__2,  p)));  
Dictionary<string,  string>  dictionary  =  PluginLoader.officePluginsDictionary;  
string  text3  =  key;  
if  (PluginLoader.<>o__12.<>p__6  ==  null)  
{  
PluginLoader.<>o__12.<>p__6  =  CallSite<Func<CallSite,  object,  string>>.Create(Binder.Convert(CSharpBinderFlags.None,  typeof(string),  typeof(PluginLoader)));  
}  
Func<CallSite,  object,  string>  target3  =  PluginLoader.<>o__12.<>p__6.Target;  
CallSite  <>p__3  =  PluginLoader.<>o__12.<>p__6;  
if  (PluginLoader.<>o__12.<>p__5  ==  null)  
{  
PluginLoader.<>o__12.<>p__5  =  CallSite<Func<CallSite,  object,  object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None,  "ToString",  null,  typeof(PluginLoader),  new  CSharpArgumentInfo[]  {  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None,  null)  }));  
}  
Func<CallSite,  object,  object>  target4  =  PluginLoader.<>o__12.<>p__5.Target;  
CallSite  <>p__4  =  PluginLoader.<>o__12.<>p__5;  
if  (PluginLoader.<>o__12.<>p__4  ==  null)  
{  
PluginLoader.<>o__12.<>p__4  =  CallSite<Func<CallSite,  object,  object>>.Create(Binder.GetMember(CSharpBinderFlags.None,  "key",  typeof(PluginLoader),  new  CSharpArgumentInfo[]  {  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None,  null)  }));  
}  
dictionary[text3]  =  target3(<>p__3,  target4(<>p__4,  PluginLoader.<>o__12.<>p__4.Target(PluginLoader.<>o__12.<>p__4,  p)));  
if  (PluginLoader.<>o__12.<>p__10  ==  null)  
{  
PluginLoader.<>o__12.<>p__10  =  CallSite<Func<CallSite,  object,  bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None,  ExpressionType.IsTrue,  typeof(PluginLoader),  new  CSharpArgumentInfo[]  {  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None,  null)  }));  
}  
Func<CallSite,  object,  bool>  target5  =  PluginLoader.<>o__12.<>p__10.Target;  
CallSite  <>p__5  =  PluginLoader.<>o__12.<>p__10;  
if  (PluginLoader.<>o__12.<>p__9  ==  null)  
{  
PluginLoader.<>o__12.<>p__9  =  CallSite<Func<CallSite,  object,  string,  object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None,  ExpressionType.Equal,  typeof(PluginLoader),  new  CSharpArgumentInfo[]  
{  
CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None,  null),  
CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType  |  CSharpArgumentInfoFlags.Constant,  null)  
}));  
}  
Func<CallSite,  object,  string,  object>  target6  =  PluginLoader.<>o__12.<>p__9.Target;  
CallSite  <>p__6  =  PluginLoader.<>o__12.<>p__9;  
if  (PluginLoader.<>o__12.<>p__8  ==  null)  
{  
PluginLoader.<>o__12.<>p__8  =  CallSite<Func<CallSite,  object,  object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None,  "ToString",  null,  typeof(PluginLoader),  new  CSharpArgumentInfo[]  {  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None,  null)  }));  
}  
Func<CallSite,  object,  object>  target7  =  PluginLoader.<>o__12.<>p__8.Target;  
CallSite  <>p__7  =  PluginLoader.<>o__12.<>p__8;  
if  (PluginLoader.<>o__12.<>p__7  ==  null)  
{  
PluginLoader.<>o__12.<>p__7  =  CallSite<Func<CallSite,  object,  object>>.Create(Binder.GetMember(CSharpBinderFlags.None,  "id",  typeof(PluginLoader),  new  CSharpArgumentInfo[]  {  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None,  null)  }));  
}  
if  (target5(<>p__5,  target6(<>p__6,  target7(<>p__7,  PluginLoader.<>o__12.<>p__7.Target(PluginLoader.<>o__12.<>p__7,  p)),  "CloudSdkPlugin")))  
{  
if  (PluginLoader.<>o__12.<>p__13  ==  null)  
{  
PluginLoader.<>o__12.<>p__13  =  CallSite<Action<CallSite,  List<string>,  object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded,  "Add",  null,  typeof(PluginLoader),  new  CSharpArgumentInfo[]  
{  
CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType,  null),  
CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None,  null)  
}));  
}  
Action<CallSite,  List<string>,  object>  target8  =  PluginLoader.<>o__12.<>p__13.Target;  
CallSite  <>p__8  =  PluginLoader.<>o__12.<>p__13;  
List<string>  cloudSDKPluginHashList  =  PluginLoader.CloudSDKPluginHashList;  
if  (PluginLoader.<>o__12.<>p__12  ==  null)  
{  
PluginLoader.<>o__12.<>p__12  =  CallSite<Func<CallSite,  object,  object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None,  "ToString",  null,  typeof(PluginLoader),  new  CSharpArgumentInfo[]  {  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None,  null)  }));  
}  
Func<CallSite,  object,  object>  target9  =  PluginLoader.<>o__12.<>p__12.Target;  
CallSite  <>p__9  =  PluginLoader.<>o__12.<>p__12;  
if  (PluginLoader.<>o__12.<>p__11  ==  null)  
{  
PluginLoader.<>o__12.<>p__11  =  CallSite<Func<CallSite,  object,  object>>.Create(Binder.GetMember(CSharpBinderFlags.None,  "key",  typeof(PluginLoader),  new  CSharpArgumentInfo[]  {  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None,  null)  }));  
}  
target8(<>p__8,  cloudSDKPluginHashList,  target9(<>p__9,  PluginLoader.<>o__12.<>p__11.Target(PluginLoader.<>o__12.<>p__11,  p)));  
}  
if  (PluginLoader.<>o__12.<>p__17  ==  null)  
{  
PluginLoader.<>o__12.<>p__17  =  CallSite<Func<CallSite,  object,  bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None,  ExpressionType.IsTrue,  typeof(PluginLoader),  new  CSharpArgumentInfo[]  {  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None,  null)  }));  
}  
Func<CallSite,  object,  bool>  target10  =  PluginLoader.<>o__12.<>p__17.Target;  
CallSite  <>p__10  =  PluginLoader.<>o__12.<>p__17;  
if  (PluginLoader.<>o__12.<>p__16  ==  null)  
{  
PluginLoader.<>o__12.<>p__16  =  CallSite<Func<CallSite,  object,  string,  object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None,  ExpressionType.Equal,  typeof(PluginLoader),  new  CSharpArgumentInfo[]  
{  
CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None,  null),  
CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType  |  CSharpArgumentInfoFlags.Constant,  null)  
}));  
}  
Func<CallSite,  object,  string,  object>  target11  =  PluginLoader.<>o__12.<>p__16.Target;  
CallSite  <>p__11  =  PluginLoader.<>o__12.<>p__16;  
if  (PluginLoader.<>o__12.<>p__15  ==  null)  
{  
PluginLoader.<>o__12.<>p__15  =  CallSite<Func<CallSite,  object,  object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None,  "ToString",  null,  typeof(PluginLoader),  new  CSharpArgumentInfo[]  {  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None,  null)  }));  
}  
Func<CallSite,  object,  object>  target12  =  PluginLoader.<>o__12.<>p__15.Target;  
CallSite  <>p__12  =  PluginLoader.<>o__12.<>p__15;  
if  (PluginLoader.<>o__12.<>p__14  ==  null)  
{  
PluginLoader.<>o__12.<>p__14  =  CallSite<Func<CallSite,  object,  object>>.Create(Binder.GetMember(CSharpBinderFlags.None,  "id",  typeof(PluginLoader),  new  CSharpArgumentInfo[]  {  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None,  null)  }));  
}  
if  (target10(<>p__10,  target11(<>p__11,  target12(<>p__12,  PluginLoader.<>o__12.<>p__14.Target(PluginLoader.<>o__12.<>p__14,  p)),  "NetviosSdkPlugin")))  
{  
if  (PluginLoader.<>o__12.<>p__20  ==  null)  
{  
PluginLoader.<>o__12.<>p__20  =  CallSite<Action<CallSite,  List<string>,  object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded,  "Add",  null,  typeof(PluginLoader),  new  CSharpArgumentInfo[]  
{  
CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType,  null),  
CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None,  null)  
}));  
}  
Action<CallSite,  List<string>,  object>  target13  =  PluginLoader.<>o__12.<>p__20.Target;  
CallSite  <>p__13  =  PluginLoader.<>o__12.<>p__20;  
List<string>  netviosSDKPluginHashList  =  PluginLoader.NetviosSDKPluginHashList;  
if  (PluginLoader.<>o__12.<>p__19  ==  null)  
{  
PluginLoader.<>o__12.<>p__19  =  CallSite<Func<CallSite,  object,  object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None,  "ToString",  null,  typeof(PluginLoader),  new  CSharpArgumentInfo[]  {  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None,  null)  }));  
}  
Func<CallSite,  object,  object>  target14  =  PluginLoader.<>o__12.<>p__19.Target;  
CallSite  <>p__14  =  PluginLoader.<>o__12.<>p__19;  
if  (PluginLoader.<>o__12.<>p__18  ==  null)  
{  
PluginLoader.<>o__12.<>p__18  =  CallSite<Func<CallSite,  object,  object>>.Create(Binder.GetMember(CSharpBinderFlags.None,  "key",  typeof(PluginLoader),  new  CSharpArgumentInfo[]  {  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None,  null)  }));  
}  
target13(<>p__13,  netviosSDKPluginHashList,  target14(<>p__14,  PluginLoader.<>o__12.<>p__18.Target(PluginLoader.<>o__12.<>p__18,  p)));  
}  
if  (PluginLoader.<>o__12.<>p__24  ==  null)  
{  
PluginLoader.<>o__12.<>p__24  =  CallSite<Func<CallSite,  object,  bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None,  ExpressionType.IsTrue,  typeof(PluginLoader),  new  CSharpArgumentInfo[]  {  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None,  null)  }));  
}  
Func<CallSite,  object,  bool>  target15  =  PluginLoader.<>o__12.<>p__24.Target;  
CallSite  <>p__15  =  PluginLoader.<>o__12.<>p__24;  
if  (PluginLoader.<>o__12.<>p__23  ==  null)  
{  
PluginLoader.<>o__12.<>p__23  =  CallSite<Func<CallSite,  object,  string,  object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None,  ExpressionType.Equal,  typeof(PluginLoader),  new  CSharpArgumentInfo[]  
{  
CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None,  null),  
CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType  |  CSharpArgumentInfoFlags.Constant,  null)  
}));  
}  
Func<CallSite,  object,  string,  object>  target16  =  PluginLoader.<>o__12.<>p__23.Target;  
CallSite  <>p__16  =  PluginLoader.<>o__12.<>p__23;  
if  (PluginLoader.<>o__12.<>p__22  ==  null)  
{  
PluginLoader.<>o__12.<>p__22  =  CallSite<Func<CallSite,  object,  object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None,  "ToString",  null,  typeof(PluginLoader),  new  CSharpArgumentInfo[]  {  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None,  null)  }));  
}  
Func<CallSite,  object,  object>  target17  =  PluginLoader.<>o__12.<>p__22.Target;  
CallSite  <>p__17  =  PluginLoader.<>o__12.<>p__22;  
if  (PluginLoader.<>o__12.<>p__21  ==  null)  
{  
PluginLoader.<>o__12.<>p__21  =  CallSite<Func<CallSite,  object,  object>>.Create(Binder.GetMember(CSharpBinderFlags.None,  "id",  typeof(PluginLoader),  new  CSharpArgumentInfo[]  {  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None,  null)  }));  
}  
if  (target15(<>p__15,  target16(<>p__16,  target17(<>p__17,  PluginLoader.<>o__12.<>p__21.Target(PluginLoader.<>o__12.<>p__21,  p)),  "PlayerDataPlugin")))  
{  
if  (PluginLoader.<>o__12.<>p__27  ==  null)  
{  
PluginLoader.<>o__12.<>p__27  =  CallSite<Action<CallSite,  List<string>,  object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded,  "Add",  null,  typeof(PluginLoader),  new  CSharpArgumentInfo[]  
{  
CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType,  null),  
CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None,  null)  
}));  
}  
Action<CallSite,  List<string>,  object>  target18  =  PluginLoader.<>o__12.<>p__27.Target;  
CallSite  <>p__18  =  PluginLoader.<>o__12.<>p__27;  
List<string>  playerDataPluginHashList  =  PluginLoader.PlayerDataPluginHashList;  
if  (PluginLoader.<>o__12.<>p__26  ==  null)  
{  
PluginLoader.<>o__12.<>p__26  =  CallSite<Func<CallSite,  object,  object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None,  "ToString",  null,  typeof(PluginLoader),  new  CSharpArgumentInfo[]  {  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None,  null)  }));  
}  
Func<CallSite,  object,  object>  target19  =  PluginLoader.<>o__12.<>p__26.Target;  
CallSite  <>p__19  =  PluginLoader.<>o__12.<>p__26;  
if  (PluginLoader.<>o__12.<>p__25  ==  null)  
{  
PluginLoader.<>o__12.<>p__25  =  CallSite<Func<CallSite,  object,  object>>.Create(Binder.GetMember(CSharpBinderFlags.None,  "key",  typeof(PluginLoader),  new  CSharpArgumentInfo[]  {  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None,  null)  }));  
}  
target18(<>p__18,  playerDataPluginHashList,  target19(<>p__19,  PluginLoader.<>o__12.<>p__25.Target(PluginLoader.<>o__12.<>p__25,  p)));  
}  
}  
if  ((PluginLoader.CloudSDKPluginHashList.Count  ==  0  &&  PluginLoader.NetviosSDKPluginHashList.Count  ==  0)  ||  PluginLoader.PlayerDataPluginHashList.Count  ==  0)  
{  
IPA.Logging.Logger.loader.Error("sdk plugin or playerData plugin not included in download plugin");  
Application.Quit();  
}  
}
```

After ```RefreshOfficePlugins``` finishes, it's ```FilterUnauthorized```'s turn.

```
// IPA.Loader.PluginLoader  
// Token: 0x060001BE RID: 446 RVA: 0x00009060 File Offset: 0x00007260  
private  static  void  FilterUnauthorized()  
{  
PluginLoader.RefreshOfficePlugins();  
bool  HasSDKPlugin  =  false;  
bool  HasPlayerDataPlugin  =  false;  
List<PluginMetadata>  enabled  =  new  List<PluginMetadata>(PluginLoader.PluginsMetadata.Count);  
foreach  (PluginMetadata  meta  in  PluginLoader.PluginsMetadata)  
{  
if  (PluginLoader.CloudSDKPluginHashList.Contains(meta.HashStr)  ||  PluginLoader.NetviosSDKPluginHashList.Contains(meta.HashStr))  
{  
HasSDKPlugin  =  true;  
}  
if  (PluginLoader.PlayerDataPluginHashList.Contains(meta.HashStr))  
{  
HasPlayerDataPlugin  =  true;  
}  
bool  passed  =  true;  
string  key  =  meta.Id.ToString()  +  "-"  +  meta.Version.ToString();  
IPA.Logging.Logger.log.Info(key  +  "="  +  meta.HashStr);  
if  (!meta.IsSelf  &&  meta.Name.ToLower()  !=  "dynamicopenvr"  &&  (!PluginLoader.officePluginsDictionary.ContainsKey(key)  ||  PluginLoader.officePluginsDictionary[key]  !=  meta.HashStr))  
{  
passed  =  false;  
PluginLoader.ignoredPlugins.Add(meta,  new  IgnoreReason(Reason.Unsupported,  null,  null,  null)  
{  
ReasonText  =  meta.Name  +  " Unauthorized"  
});  
IPA.Logging.Logger.loader.Warn(meta.Name  +  " Unauthorized");  
}  
if  (passed)  
{  
enabled.Add(meta);  
}  
}  
if  (!HasSDKPlugin  ||  !HasPlayerDataPlugin)  
{  
IPA.Logging.Logger.loader.Error("sdk plugin or playerData plugin not included plugin dir");  
Application.Quit();  
}  
PluginLoader.PluginsMetadata  =  enabled;  
}
```
The `FilterUnauthorized` method iterates through all installed plugins, containing the following line:

`if (!meta.IsSelf && ... (!PluginLoader.officePluginsDictionary.ContainsKey(key) || PluginLoader.officePluginsDictionary[key] != meta.HashStr))`

This means: if your plugin is not in the "official dictionary" it downloads from the server, or if its hash value differs from the one provided by the server, it is marked as passed = false.

Then, it print log: `meta.Name + " Unauthorized"`, and refuses to load the plugin. However, the "dynamicopenvr" plugin is hardcoded as the sole exception.

After filtering out illegal plugins, it performs a final check:

```if (!HasSDKPlugin || !HasPlayerDataPlugin) Application.Quit();```

This means that if your game does not have the specified NetviosSdkPlugin or PlayerDataPlugin (or if these two plugins are determined to be Unauthorized by the above logic), the game will crash immediately. You cannot run the game without these two plugins.


In summary, this is a heavily modified BSIPA with added online DRM, forced whitelisting, telemetry, and security backdoors. Our current task is to remove RefreshOfficePlugins and FilterUnauthorized. After removing these two sections, we successfully bypassed Netvios' DRM!

However, IPA.Netvios still exists and cannot be deleted, and there may still be potential telemetry code within this BSIPA. But we have already removed the NetviosSDKPlugin; all we need to do is prevent the software from connecting to beatsaverbbs.com.

However, this makes Netease's multiplayer games unusable (the good news is that it's no longer functional; clicking it only suspends the game).

<img width="1919" height="1079" alt="image" src="https://gist.github.com/user-attachments/assets/a917af03-3f8f-4d47-b6cc-f1a777baf2f5" />
