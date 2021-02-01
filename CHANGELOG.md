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
