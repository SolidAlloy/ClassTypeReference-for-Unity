# [2.16.0](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.15.1...2.16.0) (2022-07-24)


### Bug Fixes

* Fixed type being null when SerializeReference is used on a field next to a serializable class that uses TypeReference ([cc77a82](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/cc77a8252fd62184093ac4bd91741cf71c856243))


### Features

* Switched from GUID to assembly names in asmdefs ([9645049](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/96450497153d8d101e686f232d729ccb3b8396de))

## [2.15.1](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.15.0...2.15.1) (2022-06-07)


### Bug Fixes

* Fixed exception in ExtEvents related to TypeReference ([45ce1f8](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/45ce1f821a71e5d81bc23a7239aa872ae5505423))

# [2.15.0](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.14.0...2.15.0) (2022-05-02)


### Bug Fixes

* Made _suppressLogs work properly for all instance of TypeReference ([4034588](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/40345887de9e1c17d3496116f6a6064c8401a5b4))


### Features

* Implemented new algorithm to find the objects with the missing type ([92e8d7e](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/92e8d7e0098ad4c073d95ef43f431242a8d8a581))
* Started coloring missing types in red to draw attention to them. ([c06d230](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/c06d2301eb733a7d5e5a0277c37372ef0ce35b37))

# [2.14.0](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.13.0...2.14.0) (2022-03-07)


### Bug Fixes

* Fixed compilation errors in samples ([8a99387](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/8a9938738418e7ba4ca2c20a71a258fb0f35c15d))
* Returned the ExcludeNone parameter and marked it obsolete ([88607ee](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/88607eedf929e36cc1265c85af84ada76934ae46))


### Features

* Made ProjectSettings public ([c6f3c92](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/c6f3c926539bf83b800d0dd8f05f44456e8bb4a8))

# [2.13.0](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.12.1...2.13.0) (2022-02-08)


### Features

* Started setting GUI.changed to true when a different type is chosen in the dropdown so that one can register for 'value changed' events ([72212e2](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/72212e22049683836493dbfcfc2acfc260d91b1d))

## [2.12.1](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.12.0...2.12.1) (2022-02-03)


### Bug Fixes

* Fixed the error in console regarding the immutable Changelog file ([685a566](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/685a566997fd1aae40b305570406661018fd3f30))

# [2.12.0](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.11.5...2.12.0) (2022-02-03)


### Bug Fixes

* Fixed the package full name in the installation paragraph ([86d6547](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/86d654783ee88649b7553778c621aa7fac78d965))


### Features

* Added SerializedTypeReference.SetType(Type type) method ([a05110a](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/a05110a1bb1be0922b4905749515111e7fb06617))
* Added the ShowAllTypes property to the TypeOptions attribute ([8c45d27](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/8c45d274fdfedbec5b8422b976f05561d3d3bc67))
* Added TypeReference.GetTypeNameFromNameAndAssembly static method ([1302a1c](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/1302a1c5c1252eb6a6d58d59ffbe89ed89fe947f))
* Made TypeReference.TypeNameAndAssembly property public ([77cef13](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/77cef13004ceb9774488ed75db8437d60aff75c4))

## [2.11.5](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.11.4...2.11.5) (2021-11-09)


### Bug Fixes

* Resolved the .dll dependency conflict in Unity 2021.2 ([3779621](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/377962158009daf1fd0d17d8f3086f86ec665939))

## [2.11.4](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.11.3...2.11.4) (2021-10-24)


### Bug Fixes

* Fixed NullReferenceException when pressing keyboard keys in search mode ([da1113e](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/da1113e6e36e7819f20068b0e3447ba5734986dc))

## [2.11.3](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.11.2...2.11.3) (2021-10-22)


### Bug Fixes

* Fixed NRE when opening a type dropdown on MacOS ([ec75afb](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/ec75afb614b1b21d7bb4d9d63350ad67bbaf6a33))

## [2.11.2](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.11.1...2.11.2) (2021-10-17)


### Bug Fixes

* Fixed incorrect behavior when scrolling the hierarchy up with keyboard ([4a382fe](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/4a382fe3daaf89b82e575b96280e5412b9711890))
* Fixed NullReferenceException when choosing a type for a generic unity object ([62d141b](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/62d141be1ebe3d97884add6547eebb41a1862e46))

## [2.11.1](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.11.0...2.11.1) (2021-10-16)


### Bug Fixes

* Fixed missing extension method because of the SolidUtilties version mismatch ([bddebd1](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/bddebd1b0f22cdd0985eeaa98df595f5058beaf2))
* Fixed the Event.Use() warning message when a dropdown is closed ([0b8e94c](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/0b8e94c20fa8a7e320141a1ee83f203c33fd83cc))

# [2.11.0](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.10.1...2.11.0) (2021-10-13)


### Bug Fixes

* Fixed flickering window width when opening the dropdown ([4a1383e](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/4a1383eb60a015b52cfa27fbc5f92f0e7f60fb15))


### Features

* Added ability to move through the list of types using keyboard ([85f834a](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/85f834a7a7074b74153d765f852d84b9ce62b503))

## [2.10.1](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.10.0...2.10.1) (2021-09-29)


### Bug Fixes

* Fixed MissingReferenceException sometimes occurring on MacOS when opening dropdown ([87cf3c8](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/87cf3c869ec2cba09f381c26661d1f2791238d9f))

# [2.10.0](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.9.0...2.10.0) (2021-08-22)


### Bug Fixes

* Changed code according to changes in SolidUtilities ([e794335](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/e794335a483c3c3c86bca41fc4eac08360447368))
* When calling type cannot be found, all assemblies are loaded instead of just Assembly-CSharp and its referenced assemblies. ([bdaad4a](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/bdaad4a8c542fafda36eefdc73c1f83c5275ccb6))


### Features

* Moved SearchbarMinItemsCount and UseBuiltInNames attribute properties to Project Settings ([361e691](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/361e6911640ab6494c840fb3c46b3be1dbf3bf47))

# [2.9.0](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.8.7...2.9.0) (2021-03-23)


### Bug Fixes

* Fixed ArgumentException when using DropdownStyle outside of OnGUI ([912576e](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/912576e001a2c09de5f9d09868c0146a1b3c113a))


### Features

* Added AllowInternal option to TypeOptionsAttribute to include internal types into the dropdown ([e3b3bfa](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/e3b3bfa9734c93d0a0859c4b7d7329b7d66df5a1))
* Added DropdownWindow popup option ([48fce05](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/48fce05e906bfebec69391962779e7af9691e6fd))
* Removed the non-visible types restriction ([03ab613](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/03ab6135e3f376e480aba40164e64b941ca0809b))
* Returned the internal types restriction ([56bbfef](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/56bbfef88eebf6221f6c4340214005481ddd5d10))

## [2.8.7](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.8.6...2.8.7) (2021-03-19)


### Bug Fixes

* Downgraded CompilerServices.Unsafe from 5.0.0 to 4.5.3 ([0b513ac](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/0b513ac082a0737171dbaf193bea238ff4a83346))
* Fixed FileNotFound exception when Assembly-CSharp is not generated in a project ([00ee76d](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/00ee76d2b3bb269c69b19df68debdcb2a83badf7))

## [2.8.6](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.8.5...2.8.6) (2021-02-19)


### Bug Fixes

* Fixed compilation errors for Unity 2019 ([7fa0e0a](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/7fa0e0a0b5e7f11f3431b0839d8ebf1c1296203a))
* Removed System.Numerics.Vectors DLL file ([734d234](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/734d234fafe707032a9512eddb8f0fdfeb391b7b))

## [2.8.5](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.8.4...2.8.5) (2021-02-05)


### Bug Fixes

* Fixed attribute options not being applied to the dropdown ([93dd61b](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/93dd61b2e283f0d8aa0cf8d1f562116d17dcbab3))

## [2.8.4](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.8.3...2.8.4) (2021-02-01)


### Bug Fixes

* Fixed incorrect define constraints in managed plugins ([436d009](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/436d009b3c33d377f90c2d013b3ef153deae02a3))

## [2.8.3](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.8.2...2.8.3) (2021-02-01)


### Bug Fixes

* Fixed "type 'MonoScript' is defined in a not referenced assembly" in Unity 2019 ([7fcd735](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/7fcd735c5072c3e536ed614b1c26d35a4e6224e6))

## [2.8.1](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.8.0...2.8.1) (2021-01-30)


### Bug Fixes

* Fixed compilation errors in Usage Examples and applied a few minor fixes to them ([eec6fae](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/eec6faef4abe6d9f95c2eca10fb9be6ad98a5c16))
* Fixed dropdown window position resetting if the distance to bottom of the screen is less than 100 pixels ([1d3eb5c](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/1d3eb5c443a7ee7c41460320167bd614154f1282))
* Renamed DLLs so that they don't cause conflicts ([bbc1af1](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/bbc1af1b8b2a66709f18d7469f8779c817f67395))

# [2.8.0](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.7.0...2.8.0) (2021-01-30)


### Bug Fixes

* Fixed ArgumentOutOfRange exception when drawing a field with no type. ([faa54e2](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/faa54e269cda7500811c74595f5e15a4e3e7b4b2))
* Fixed errors at the build compilation ([26ba65e](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/26ba65eda57564f4a1187f3b0b616be247fae5a6))
* Started allocating less garbage by using new methods from SolidUtilities ([c368a87](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/c368a874715945c3e781d8d8535e68cf393850ee))
* Started using SolidUtilities AssetSearcher.GetClassGUID method instead of implementing its own ([dac05c3](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/dac05c3e87e30e2a5e657494b2e8cb0a70c69eef))


### Features

* Added optional GUID parameter to the constructor and added the TypeRestoredFromGUID event ([5e625da](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/5e625da23e64e2446f48281f23ae5d65f4e5fd5a))
* Allowed making additional actions on type selection and manually triggering dropdown ([209d461](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/209d4614f457808241acf9f725d4f8c867649575))
* Made the InheritsAttribute constructor accept null arrays ([cf7ca6e](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/cf7ca6e5c11cc3228d9f386dec4f996901ac6618))


### Performance Improvements

* Decreased serialization time of TypeReference ([0ae12b7](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/0ae12b798a1080f2e2d7c0df5648186776e00890))
* Minimized memory allocations in TypeFieldDrawer.Draw() and DropdownWindow.OnGUI() ([1e3d433](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/1e3d433b9e8f03c278049545b15c1a8d5e901651))
* Minor CPU and garbage improvement in TypeNameFormatter ([fd184c2](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/fd184c2a695f387b9ef83434b5d9d9be1955eb56))
* Removed an accidental NUnit using statement ([8da61e8](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/8da61e87af9f5b293edae1094939448de78ba011))
* Replaced System.Guid with UnityEngine.GUID ([e5631e3](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/e5631e3d3595d19ac4daaf0db5a734467e11eb7c))
* Significantly decreased time spent on sorting items when showing a drop-down window ([c0e1fd4](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/c0e1fd4cc1a080377abeb619bcbb975540c0b911))
* Slightly decreased deserialization time of TypeReference ([3ba1566](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/3ba1566d92428dc67b93daa593a4c3fdc522f093))
* Vastly increased speed of CalculatePopupWidth() ([d07ac32](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/d07ac32c227491d58d608aff4bd01287674bff70))

# [2.7.0](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.6.6...2.7.0) (2020-12-03)


### Bug Fixes

* Fixed the dropdown window position resetting to 0 after its creation for some users. ([f3e1f66](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/f3e1f665968504e4aeb8e0bc2ffb4ce131926006))
* Fixed the warnings when opening the Usage Sample scene ([b2c5568](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/b2c55689aea08803a4a4491f3bc495ddb9d15622))
* Fixed TypeReference deserialization issue in IL2CPP builds ([f912853](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/f912853e64c8add9775b209b54093a02f9a6097d))
* Made the interface change immediate when search string is cleared ([76daf56](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/76daf560a525f7c3f6adb5de4dff933c8c7c09b6))
* Started setting the correct position.x at the window creation. ([44d80f1](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/44d80f1599638c6d57b9a4c00e77bd6a85fb1ed8))


### Features

* Added reporting of the objects that have the missing type ([0bba3e6](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/0bba3e6bc4070a0728d10415633569890d105ea1))

## [2.6.6](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.6.5...2.6.6) (2020-11-30)


### Bug Fixes

* Fixed TypeReference deserialization issue in IL2CPP builds ([4a34093](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/4a34093e9657486e97b89044c121c93a28d35c1c))

## [2.6.5](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.6.4...2.6.5) (2020-11-30)


### Bug Fixes

* Made window height non-zero on the drop-down creation. ([dc87667](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/dc876670785e02e55930262aa7a854922fab26f4))


### Performance Improvements

* Reduced the number of Resize calls inside AdjustSizeIfNeeded to one. ([dea3048](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/dea3048f29090d45b736609deae4be1363b93ebe))

## [2.6.3](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.6.2...2.6.3) (2020-11-27)


### Bug Fixes

* Fixed the dropdown showing up in the top left corner instead of the mouse position ([31ce991](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/31ce991a43d470e0ca280634daf3d02c29a3437f))

## [2.6.2](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.6.1...2.6.2) (2020-11-16)


### Bug Fixes

* Optimized the Type property setter ([2f2a7f7](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/2f2a7f73a03da0bfecf0d4a8ea9406bbf209b31c))

# [2.6.0](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.5.3...2.6.0) (2020-11-03)


### Features

* Added 'suppressLogs' optional parameter to the TypeReference constructors ([2d7a2b5](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/2d7a2b5eb377f579dc8e5f59471f4ee59f0b6b92))

## [2.5.2](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.5.1...2.5.2) (2020-11-01)


### Bug Fixes

* Added a delayed method to log a warning after unsucessful deserialization ([81e1770](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/81e1770abebe74ba94ec3edf29f10523aed2dece))
* Adjusted the logic of TypeReferenceComparer ([554fc6f](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/554fc6f46323c29922ec9eb8452d0c1490595fc7))
* GUID is now emptied if the type was not found, so that it is not searched for again. ([b23cb92](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/b23cb929184b14e9d680a5afbd9d1b19830cf104))
* Improved TypeReferenceComparer hashcode generator ([5c2ab4f](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/5c2ab4fa60cfdc07187517b5bb82e50fc8862cc5))
* Started using better method to find class type of the asset. ([bddb117](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/bddb11703d96d66134b48163f21b0a035e2e7a5f))
* The renamed type is now found as soon as all inspectors are updated ([36fbd1f](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/36fbd1f20a489f942945222916c9dc9582a60e79))

## [2.5.1](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.5.0...2.5.1) (2020-10-17)


### Bug Fixes

* Changed return type from array to list for the GetAssembliesTypeHasAccessTo method ([3f2025e](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/3f2025e90193a82a526a938c333395ae5a65e1d3))

# [2.5.0](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.4.0...2.5.0) (2020-10-16)


### Features

* Added an additional construct for the Inherits attribute ([b0a1444](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/b0a14449957aaaca511de346782d55071827b1ea))

# [2.4.0](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.3.1...2.4.0) (2020-10-16)


### Features

* Allowed adding more than one base type for the Inherits attribute ([4afd2d5](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/4afd2d5bd137e3449d90bb4487990dbcbd473695))

# [2.3.0](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.2.3...2.3.0) (2020-10-13)


### Bug Fixes

* Replaced AdditionalFilter with SerializableOnly option ([bbf9731](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/bbf973145405af66cd0e0568494493a2503cfe0d))


### Features

* Added the AdditionalFilter attribute option ([194db0b](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/194db0b80f016e8274b9f5156849798fe41065e9))
* Added TypeReference array comparer ([c674f8a](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/c674f8a787c6b1e6f5d254beede3594dffa4c022))

## [2.2.3](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.2.2...2.2.3) (2020-10-04)


### Bug Fixes

* Made TypeReferenceComparer.GetHashCode not accept null Type ([3e388b9](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/3e388b91313ac183fe61b328c80f6538ef20003b))
* Removed unnecessary assignment of type when showing the dropdown window ([d4399e0](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/d4399e0d48fd745f3728f902e063269cc43bd80d))

## [2.2.2](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.2.1...2.2.2) (2020-10-04)


### Bug Fixes

* Made the window appear wherever the cursor is pressed ([27bd3a1](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/27bd3a1d486c1a7125d020989a0351a209601abb))

# [2.2.0](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/compare/2.1.0...2.2.0) (2020-10-02)


### Features

* Implemented the UseBuiltInNames option in the drawing classes ([d379f29](https://github.com/SolidAlloy/ClassTypeReference-for-Unity/commit/d379f29679cce7cf9d6bb24fdf48b8429f70ac71))
