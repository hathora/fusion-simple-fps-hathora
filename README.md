# **Fusion Simple FPS - Hathora Integration Documentation**

The Simple FPS project demonstrates how to create a fully functional multiplayer game using [Photon Fusion 2](https://www.photonengine.com/fusion) and [Hathora](https://hathora.dev/?utm_source=photon&utm_medium=banner&utm_campaign=2024) server orchestration.

*This project is focused on setting up server hosting for Simple FPS, for initial setup see* [*Photon's Simple FPS docs*](https://doc.photonengine.com/fusion/current/game-samples/simple-fps#:~:text=The%20Simple%20FPS%20game%20is,own%20first%2Dperson%20shooter%20games.)*.*

Before continuing, review these requirements:

- You must create a Photon account and a Photon Fusion 2 Application Id.
- You must create a Hathora account.
- You must use Unity Editor 2022.3.

Note: Photon Fusion SDK is a third-party dependency and is subject to [Exit Games' license terms](license.txt).

# **Project setup**

## **Installing the Unity Editor**

To work with the Fusion Simple FPS project, you must use [Unity Editor 2022.3 LTS](https://unity.com/releases/editor/qa/lts-releases). See [Installing Unity](https://docs.unity3d.com/Manual/GettingStartedInstallingUnity.html) to learn how to install the Unity Editor for your operating system.

Required components:

- Linux Build Support (IL2CPP)
- Linux Dedicated Server Build Support

**Note**: When installing the Unity Editor, select both components ****from the components list. Otherwise, you won’t be able to build the standalone Linux binary.

[https://lh7-us.googleusercontent.com/JwvhQIQRYAsBgG25Yt7xv3TvAl_spNwKZzkRDze2qlLFHQUOzeIzjdtyBoqvj385CDAsoJyLdDt2xYBl3b-F_51NcE_JL1JNGqRGzznfn6KUb-4R0YzOI9X5a9YBiCgWrgGYPH4FQmqiuDtA6pwPV2w](https://lh7-us.googleusercontent.com/JwvhQIQRYAsBgG25Yt7xv3TvAl_spNwKZzkRDze2qlLFHQUOzeIzjdtyBoqvj385CDAsoJyLdDt2xYBl3b-F_51NcE_JL1JNGqRGzznfn6KUb-4R0YzOI9X5a9YBiCgWrgGYPH4FQmqiuDtA6pwPV2w)

## **Get started with Photon Fusion 2**

If you don’t already have one, you’ll need to [create a Photon account](https://id.photonengine.com/en-US/Account/SignUp) to start using Photon Fusion. After you have an account, log into the [Photon Dashboard](https://dashboard.photonengine.com/en-US/) and create a new **Fusion 2** application.

**Note**: See the [Photon Fusion Introduction](https://doc.photonengine.com/fusion/v2/getting-started/fusion-introduction) if you have trouble getting started.

1. From the Photon Dashboard, select **Create a new app**.
    
    [https://lh7-us.googleusercontent.com/FXrODTrpb8SDq6AX8FwtU12Kd0paJ1PaZd-1HMKwaoLkDQtcxULdU8Ew-kqoK38gc0ljsZJ0aC_KJaFm85lHn0_Ff8uFTJFI5MqT0mX8lIzni_Rk367a1jIbitnsGc05LFA5J6rXbM_lpoF5fKjlyO0](https://lh7-us.googleusercontent.com/FXrODTrpb8SDq6AX8FwtU12Kd0paJ1PaZd-1HMKwaoLkDQtcxULdU8Ew-kqoK38gc0ljsZJ0aC_KJaFm85lHn0_Ff8uFTJFI5MqT0mX8lIzni_Rk367a1jIbitnsGc05LFA5J6rXbM_lpoF5fKjlyO0)
    
2. Set **Photon SDK** to **Fusion**.
    
    [https://lh7-us.googleusercontent.com/4yaBkRlVxOiS_8mPUbc75ezxhsloCS8a37SfpS7UeRrEnkV1OZKenoBtwen5cvLzEtp5zXmFrhSDv0Afpz2bw_Nf0Cnnjr_jExIL2EWRIUqQz33CcOgWoCTC9HL2mu8jt7jenN9m-5Lyj1kSNpnLYbQ](https://lh7-us.googleusercontent.com/4yaBkRlVxOiS_8mPUbc75ezxhsloCS8a37SfpS7UeRrEnkV1OZKenoBtwen5cvLzEtp5zXmFrhSDv0Afpz2bw_Nf0Cnnjr_jExIL2EWRIUqQz33CcOgWoCTC9HL2mu8jt7jenN9m-5Lyj1kSNpnLYbQ)
    
3. Set **SDK Version** to **Fusion 2.**
4. Name the application.
5. Optionally, provide a brief description and URL.
6. Select **Create**.
7. After creating the application, select **App ID** in the Photon Dashboard and copy it.

[https://lh7-us.googleusercontent.com/15dXb3GS4n46ZOndlMm-urw5SK3dkJLomOOFdaFCE5jKsCHSi2R0SqlY4JfnHAvw99bc_LipbSjP6Of_IuvIJ6aQNOAnUYFN0dTKxdDvIo_zCMAdmx7andNVa8mcNtyuuQsDY67t16rK1KLTBSaZ7GQ](https://lh7-us.googleusercontent.com/15dXb3GS4n46ZOndlMm-urw5SK3dkJLomOOFdaFCE5jKsCHSi2R0SqlY4JfnHAvw99bc_LipbSjP6Of_IuvIJ6aQNOAnUYFN0dTKxdDvIo_zCMAdmx7andNVa8mcNtyuuQsDY67t16rK1KLTBSaZ7GQ)

## **Linking the Photon Fusion 2 project**

Download the Fusion Simple FPS, then launch it in the Unity Editor.

1. Select **Tools** > **Fusion** > **Fusion Hub**.
    
    [https://lh7-us.googleusercontent.com/LS42ckv47q5skPNBrpHaPYER4iZrHNPNBw-tqhOC2DRV1e4HAIsPsR8R454UC-KVrDo7mI6dzuwO-cg47WoulfHi1Jw6M2AEtsS1eyfuNKruqGIJMAa6geYzplYQn40yyz8VaA7NmOR2I_-mMDsIRHk](https://lh7-us.googleusercontent.com/LS42ckv47q5skPNBrpHaPYER4iZrHNPNBw-tqhOC2DRV1e4HAIsPsR8R454UC-KVrDo7mI6dzuwO-cg47WoulfHi1Jw6M2AEtsS1eyfuNKruqGIJMAa6geYzplYQn40yyz8VaA7NmOR2I_-mMDsIRHk)
    
2. Paste the App ID you copied into the **Fusion App Id** field and confirm by **Enter**.
    
    [https://lh7-us.googleusercontent.com/d0CSpOZB6fn_mC93qKrlgPPhwsi5lWRFDpMV8wqW235mk6if0KWMw-eYBWgaa-K6M8S-S8tXDrWg_cF2HyRV7Knfimf2Scq37LEt0zH5UG2A3F0UXAtYkCj9kJeFPvceM4GfbDCWhY4kr2LI2-dZXnY](https://lh7-us.googleusercontent.com/d0CSpOZB6fn_mC93qKrlgPPhwsi5lWRFDpMV8wqW235mk6if0KWMw-eYBWgaa-K6M8S-S8tXDrWg_cF2HyRV7Knfimf2Scq37LEt0zH5UG2A3F0UXAtYkCj9kJeFPvceM4GfbDCWhY4kr2LI2-dZXnY)
    

## **Get started with Hathora Cloud**

Simple FPS dedicated servers can be deployed globally on Hathora Cloud. With Hathora Cloud, you get some key benefits:

- Automated scalability and server orchestration
- Immediate access to 11 global regions (map nicely with Photon Cloud regions)
- Simple integration process
- Configure, build, and deploy directly from Hathora’s Unity editor plugin

Relevant files for Hathora integration can be found:

- ***Assets/Hathora***
    - This is the installed [Hathora Unity plugin](https://github.com/hathora/unity-plugin).
- ***Assets/Photon/FusionAddons/Hathora***
    - Contains Hathora <=> Fusion game independent integration.
- ***Assets/Scripts/Hathora***
    - Contains integration files specific to Simple FPS.

To enable integration:

1. Create a Hathora Cloud account at [](https://console.hathora.dev/)[https://console.hathora.dev](https://console.hathora.dev/?utm_source=photon&utm_medium=banner&utm_campaign=2024)

[https://lh7-us.googleusercontent.com/72_xb-zdh5MXEBrmK-6SRx7aP30uRjUxI_p_MTwLvBrXJIf5YJmQ1Y3zPh7l2Zx-A7k92PZ2QhOhLXK2xpn3td33QyVuO_zA1AWPxIgEiPA4dimeVkPpd7KxYSHvvA0PtuuWI9mTHKHNXQLNlshBlss](https://lh7-us.googleusercontent.com/72_xb-zdh5MXEBrmK-6SRx7aP30uRjUxI_p_MTwLvBrXJIf5YJmQ1Y3zPh7l2Zx-A7k92PZ2QhOhLXK2xpn3td33QyVuO_zA1AWPxIgEiPA4dimeVkPpd7KxYSHvvA0PtuuWI9mTHKHNXQLNlshBlss)

1. Select the ***HathoraServerConfig*** asset included in the project (***Assets/Photon/FusionAddons/Hathora/Configs***).
2. Log in with your Hathora Cloud account - this will open a web browser window to complete login.

[https://lh7-us.googleusercontent.com/mvCu7wjIUrBxZlJctpcmdDOZ92_VqEQz1yLZv7QQjX_MNo5cUO7eIDbTHE2SmX_nTev4vwdq_PNk9IrWHp9xFeL6q45dbbkBY9mRznCYNNS1oaHWnOcT8TivUeDGtzyYlFy6BTdUw-kN5OtLBEsLZwU](https://lh7-us.googleusercontent.com/mvCu7wjIUrBxZlJctpcmdDOZ92_VqEQz1yLZv7QQjX_MNo5cUO7eIDbTHE2SmX_nTev4vwdq_PNk9IrWHp9xFeL6q45dbbkBY9mRznCYNNS1oaHWnOcT8TivUeDGtzyYlFy6BTdUw-kN5OtLBEsLZwU)

1. Create a new application.

[https://lh7-us.googleusercontent.com/ksRPhfowWT53DiAgmoNjvzohVX7xWREEpIkhLh0PTnsCU_eGH4aW2JGuChDETxn_T4USSIJfFLh6AGxvQWeK1eldQ-nNBjsCTdkpXJfNZL6Cuiqz80C8QRFKY1Y7hpai6JIACK-grRirVgK8XWGeX1A](https://lh7-us.googleusercontent.com/ksRPhfowWT53DiAgmoNjvzohVX7xWREEpIkhLh0PTnsCU_eGH4aW2JGuChDETxn_T4USSIJfFLh6AGxvQWeK1eldQ-nNBjsCTdkpXJfNZL6Cuiqz80C8QRFKY1Y7hpai6JIACK-grRirVgK8XWGeX1A)

[https://lh7-us.googleusercontent.com/ccrjji_HDYoLVb6UqVZafgJMHRDn2T-cdz55yprEvKhxUVFoJAJTghO8U6uR3HEErdLlc8CvPwyGYaz7uviO1fwuT9Xp_LUA1H9KX57YX1MmpDrlqSS2Q_O8fTwOJeC-Gv23Or3UDQ2lLtkZVWDHE7o](https://lh7-us.googleusercontent.com/ccrjji_HDYoLVb6UqVZafgJMHRDn2T-cdz55yprEvKhxUVFoJAJTghO8U6uR3HEErdLlc8CvPwyGYaz7uviO1fwuT9Xp_LUA1H9KX57YX1MmpDrlqSS2Q_O8fTwOJeC-Gv23Or3UDQ2lLtkZVWDHE7o)

1. Now in the Unity plugin, you can:
    - Refresh your application list (1)
    - Select your new application (2)
    - Copy the App Id (3)

[https://lh7-us.googleusercontent.com/qY_B3lJ-lnqHEJrIjhmgNLhjnIQZfTM2cwf0dvAZX_ZVNLyHFRYfMHMMJqvpNfIhQJtso_3Ht_WCKEGLU9reLfq37_K5IXsqo6jwFlqo7ObIC5OZj-nb7VRKM0r4RB067RpvLsFWkjRHqWPxCSEurU0](https://lh7-us.googleusercontent.com/qY_B3lJ-lnqHEJrIjhmgNLhjnIQZfTM2cwf0dvAZX_ZVNLyHFRYfMHMMJqvpNfIhQJtso_3Ht_WCKEGLU9reLfq37_K5IXsqo6jwFlqo7ObIC5OZj-nb7VRKM0r4RB067RpvLsFWkjRHqWPxCSEurU0)

1. Select the ***HathoraClientConfig*** asset included in the project (***Assets/Photon/FusionAddons/Hathora/Configs***) and update the App Id.

[https://lh7-us.googleusercontent.com/jsCikWpAvg6oVZhkFGEoPrzBi7Egx8gf1VwWv1PwaZ4VxIORiH5SsvOrXj_ScfNHkfm0ftyHJfEWr5COVMw5z98cI9k5Zyto9KanyN1k9Xe1ctIyplctJRqVl21UO8rpt7bbO4ebk4Gp8XpMpVAMIx4](https://lh7-us.googleusercontent.com/jsCikWpAvg6oVZhkFGEoPrzBi7Egx8gf1VwWv1PwaZ4VxIORiH5SsvOrXj_ScfNHkfm0ftyHJfEWr5COVMw5z98cI9k5Zyto9KanyN1k9Xe1ctIyplctJRqVl21UO8rpt7bbO4ebk4Gp8XpMpVAMIx4)

1. Select ***HathoraServerConfig*** and **Generate Server Build.**
    
    [https://lh7-us.googleusercontent.com/UdrjXr1wB7ie6SuXIlAZ87NjUL8ITntmiC9Sm2shqrxmEVeGOtMW9DwAdvXHe2HuHhAziLpdjBCNRmZn_Bx1ELQVGsbq1M1ZqdGj6JPxmA79FC-kmPJ_dnhYiSLGXEQdLwxW73p3JH-GpzSdN8nPQ0Q](https://lh7-us.googleusercontent.com/UdrjXr1wB7ie6SuXIlAZ87NjUL8ITntmiC9Sm2shqrxmEVeGOtMW9DwAdvXHe2HuHhAziLpdjBCNRmZn_Bx1ELQVGsbq1M1ZqdGj6JPxmA79FC-kmPJ_dnhYiSLGXEQdLwxW73p3JH-GpzSdN8nPQ0Q)
    

**Note:** Sometimes the build fails due to platform switching or loading Linux platform toolchain packages in the background. To resolve it, try restarting the Unity editor, switch to the **Dedicated Server** - **Linux** platform and start the build manually. Make sure the output path matches *Build directory* and *Build file name*, in this case ***[PROJECT_ROOT]/Build-Server/Hathora-Unity_LinuxServer.x86_64***

[https://lh7-us.googleusercontent.com/sea8yHxWYUaobAfUJoOa6u5rLikwUfyo0jqqbM46wI5eGZ2zpK8LyoHTcS1OOWycrTpZwSU-vN36zJ3FyEORZF4AIY-MLoOT8dAzS2uA0s6Zc9D72MBKGyIWWnMWJ8x4vQdb6CaXZKWhh8ipv0iw0Cw](https://lh7-us.googleusercontent.com/sea8yHxWYUaobAfUJoOa6u5rLikwUfyo0jqqbM46wI5eGZ2zpK8LyoHTcS1OOWycrTpZwSU-vN36zJ3FyEORZF4AIY-MLoOT8dAzS2uA0s6Zc9D72MBKGyIWWnMWJ8x4vQdb6CaXZKWhh8ipv0iw0Cw)

1. Deploy server build to Hathora Cloud.
    
    [https://lh7-us.googleusercontent.com/RETkYoofL42YOZi3q3xttyLd7jmjCvfnEdKVhHF4V_zSKzhbgVZ-IWnVjstBO63fZ2qAo11jydhYBLIQ8gdYWeOHWgTM8Xoj0Fd-2fL7U5PRzOOMf8Fi6Zf1Z53mLciHzkKWFIJHiY3B7SZYEMVf4yw](https://lh7-us.googleusercontent.com/RETkYoofL42YOZi3q3xttyLd7jmjCvfnEdKVhHF4V_zSKzhbgVZ-IWnVjstBO63fZ2qAo11jydhYBLIQ8gdYWeOHWgTM8Xoj0Fd-2fL7U5PRzOOMf8Fi6Zf1Z53mLciHzkKWFIJHiY3B7SZYEMVf4yw)
    
2. Now you should see the *Latest deployment* being updated in Hathora Cloud
    
    [https://lh7-us.googleusercontent.com/jsuvzIVrxeFrAOFvSHbOqGpXi9kuYSjh4f8nzIevYWh_p7vUTQ1rWsFN4fZbFvBcZocY7y8rlrMq2bFGIu568XeQdUYBHGhlN0tHDKz6SOeYR5LVDVvPg0qLI8eGybBl-HI3jc7t_bHLOlwY6qCc6C0](https://lh7-us.googleusercontent.com/jsuvzIVrxeFrAOFvSHbOqGpXi9kuYSjh4f8nzIevYWh_p7vUTQ1rWsFN4fZbFvBcZocY7y8rlrMq2bFGIu568XeQdUYBHGhlN0tHDKz6SOeYR5LVDVvPg0qLI8eGybBl-HI3jc7t_bHLOlwY6qCc6C0)
    

# **Building & starting the game client**

Creating a client build is no different from creating any other regular build.

1. Select your target platform and make a build.
2. When the build is done, run two instances of the build.
3. Press ***Quick Play*** on one client and wait until the game is loaded. It can take a while if you are running for the first time.

[https://lh7-us.googleusercontent.com/rhAk9keyLClXbnM1pF3GoIzZLHudX-pXVM29oAuQVDN_rD8fhsu6DWNibndTIjIydLqi8GgKnMObqB0tiiM30KIVjVf9iPRbogR9tJm-kkRLey5HUDpYbR2gzyI2xrsX-JnpWJNFSqw-AtNNMSaVWcw](https://lh7-us.googleusercontent.com/rhAk9keyLClXbnM1pF3GoIzZLHudX-pXVM29oAuQVDN_rD8fhsu6DWNibndTIjIydLqi8GgKnMObqB0tiiM30KIVjVf9iPRbogR9tJm-kkRLey5HUDpYbR2gzyI2xrsX-JnpWJNFSqw-AtNNMSaVWcw)

1. Press ***Quick Play*** on the second client and it should connect you into the same room.
2. Check Hathora Cloud, you’ll be able to see server instance details.

[https://lh7-us.googleusercontent.com/HdJwYC8g0U25A7lONSntnsy983Vq3v1OGuC11QzRJ4imE117hw2J0A2y1vPt_SfdnPiwIXZpF3zadfm5oOU-bXZiNbD2kskyuhxDPZ_etZufM2HSIXwxD5hcJYrO3jCCQGPgF3PQAAvWdIxo6B1lZlo](https://lh7-us.googleusercontent.com/HdJwYC8g0U25A7lONSntnsy983Vq3v1OGuC11QzRJ4imE117hw2J0A2y1vPt_SfdnPiwIXZpF3zadfm5oOU-bXZiNbD2kskyuhxDPZ_etZufM2HSIXwxD5hcJYrO3jCCQGPgF3PQAAvWdIxo6B1lZlo)

1. Congratulations, you’ve just finished basic integration and are ready to iterate your own game!

# **A1: Multi-peer mode**

Fusion supports running multiple server instances within a single process, called [multi-peer mode](https://doc.photonengine.com/fusion/v2/manual/testing-and-tooling/multipeer). This feature allows for efficient CPU utilization and reduces overall cost.

1. Make sure the **Peer Mode** is set to **Multiple** in ***NetworkProjectConfig*** asset (***Assets/Photon/Fusion/Resources***).

[https://lh7-us.googleusercontent.com/k0-6WyUxP-fBVCbiCK4JN3ikR1-mHJfo6nuKc5yUyNCeZP9UNT9arVmZ-9xLqqPOUQKNindvaJFOiZ2yQdAcQjxZ4vJ8Xo6KTYaALAXva-55ONR3u6cjyvQ3UUV7paxdImsJDHlIXqlw1LMXrWgTHRw](https://lh7-us.googleusercontent.com/k0-6WyUxP-fBVCbiCK4JN3ikR1-mHJfo6nuKc5yUyNCeZP9UNT9arVmZ-9xLqqPOUQKNindvaJFOiZ2yQdAcQjxZ4vJ8Xo6KTYaALAXva-55ONR3u6cjyvQ3UUV7paxdImsJDHlIXqlw1LMXrWgTHRw)

1. Configure your application **Settings** in Hathora Cloud:
    1. Set the **Number of rooms per process** to **3**.
    2. Add transport configurations and set **Port** and **Name** properties. Make sure it is configured exactly the same as in the following image.

[https://lh7-us.googleusercontent.com/IKq-6OLhhcg7xD6-stnie2URAve-dmK0LHFfxFww9iDIS23pGhpAofC0keD8YojNsbgrK1c_7lCnMQukxDXNtVvgPwHDQXCxA7Lj4rsEiLRPHn4yDl5qJoBVo1tj0Tf3KDgnRP3wF-WkPJ11Aplo8Jk](https://lh7-us.googleusercontent.com/IKq-6OLhhcg7xD6-stnie2URAve-dmK0LHFfxFww9iDIS23pGhpAofC0keD8YojNsbgrK1c_7lCnMQukxDXNtVvgPwHDQXCxA7Lj4rsEiLRPHn4yDl5qJoBVo1tj0Tf3KDgnRP3wF-WkPJ11Aplo8Jk)

1. Select ***HathoraServerConfig*** in the Unity editor.
2. Generate a new Linux dedicated server build (check sections above for more details).
3. Configure the config as in the following image and deploy.

[https://lh7-us.googleusercontent.com/Y8NVUvkxUzwgYMFSq1BwX865bkjk467vciMUm0nh8fffglBJ3cNR2GpRs1o1wO-A2cBHQdzoFUz8AaNeY_lAaOKpGQK4ewPysjlTniZZHn92o2ToFYibjoXhm4Vqy7NiH7-PdrFI3swhHef8G04qx-0](https://lh7-us.googleusercontent.com/Y8NVUvkxUzwgYMFSq1BwX865bkjk467vciMUm0nh8fffglBJ3cNR2GpRs1o1wO-A2cBHQdzoFUz8AaNeY_lAaOKpGQK4ewPysjlTniZZHn92o2ToFYibjoXhm4Vqy7NiH7-PdrFI3swhHef8G04qx-0)

1. After joining with enough clients, you’ll be able to see on Hathora Cloud multiple rooms / games running within a single process.

[https://lh7-us.googleusercontent.com/1CDd3d5TmBd5ZTlWUBrkwkTusslldi2-5xOMsTnerQZxMhmWsb4BSc0IIbSMJzwKLrlXKDeSl5zVCA4WlLWzOa1QN-4z4pEGYKFmoNBuSUPt8g8cui_ZKBwv1VTm4uZnrcbuTAC7Rx03bFnSy615Jog](https://lh7-us.googleusercontent.com/1CDd3d5TmBd5ZTlWUBrkwkTusslldi2-5xOMsTnerQZxMhmWsb4BSc0IIbSMJzwKLrlXKDeSl5zVCA4WlLWzOa1QN-4z4pEGYKFmoNBuSUPt8g8cui_ZKBwv1VTm4uZnrcbuTAC7Rx03bFnSy615Jog)

# **A2: References**

This guide covers basic configuration and operation of Simple FPS on Hathora. We recommend to explore following topics which are related:

- [Simple FPS documentation](https://doc.photonengine.com/fusion/v2/game-samples/simple-fps) - provides general information about Fusion Simple FPS project and its architecture overview.
- [Simple KCC documentation](https://doc.photonengine.com/fusion/v2/addons/simple-kcc) - provides more information about Fusion Simple Kinematic Character Controller addon used in Simple FPS and its configuration.
- [Hathora documentation](https://hathora.dev/docs?utm_source=photon&utm_medium=banner&utm_campaign=2024) - provides more information about Hathora Cloud configuration and additional topics not covered by this guide:
    - Containerization with Docker files.
    - Integration logic walk-through.
    - Troubleshooting tips.
