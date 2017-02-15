// libraryTest1.h

#pragma once

#if UNITY_METRO 
#define EXPORT_API __declspec(dllexport) __stdcall 
#elif UNITY_WIN 
#define EXPORT_API __declspec(dllexport) 
#else 
#define EXPORT_API 
#endif

extern "C" __declspec(dllexport) 
int ** fillArray(int size);

extern "C" __declspec(dllexport) 
float FooPluginFunction();

extern "C" __declspec(dllexport) 
double Add(double a, double b);

extern "C" __declspec(dllexport) 
void* classTest_create();

extern "C" __declspec(dllexport)
int classTest_getCpt(void * obj);

extern "C" __declspec(dllexport)
void classTest_addOffest(void * obj, int value);
