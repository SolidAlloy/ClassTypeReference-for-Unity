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
