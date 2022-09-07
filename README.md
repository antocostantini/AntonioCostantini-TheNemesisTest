
# AntonioCostantini-TheNemesisTest

This is a prototype of an online multiplayer game made with Unity using Photon PUN 2.


## Technologies

- [Unity 2021.3.3f1 LTS](https://unity3d.com/get-unity/download/archive)
- [Photon PUN 2](https://www.photonengine.com/pun)
- [Input System](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/Installation.html)
- [DOTween](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676)
- [ParrelSync](https://github.com/VeriorPies/ParrelSync)
## Features

- Main menu
- Player's custom username
- Quick matchmaking
- Team selection in real time
- Players and ball physics synchronized over the network
- Score system
- Quick rematch right after the game ends
- Disconnections handling

The local username is saved using PlayerPrefs so that every time the game is launched the username will be automatically restored.

## Notes

The development was tested using the ParrelSync plugin.

This was removed on the latest commit to avoid problems when importing the project on a computer that doesn't have Git installed on it,
since it's not a plugin present on the Unity Asset Store.