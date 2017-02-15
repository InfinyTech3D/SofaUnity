// This is the main DLL file.

#include "stdafx.h"

#include "libraryTest1.h"

#include <stdlib.h> 
#include <math.h> 
#include "classTest.h"

int ** fillArray(int size) 
{
	int i = 0, j = 0;
	int ** array = (int**)calloc(size, sizeof(int*));
	for (i = 0; i < size; i++) 
	{
		array[i] = (int*)calloc(size, sizeof(int));
		for (j = 0; j < size; j++) 
		{
			array[i][j] = i * size + j;
		}
	}
	return array;
}

float FooPluginFunction()
{
	return 17.f;
}

double Add(double a, double b)
{
	return a + b;
}

void * classTest_create()
{
	return new classTest();
}

int classTest_getCpt(void * obj)
{
	classTest* cObj = (classTest*)obj;
	if (cObj)
		return cObj->getCpt();
	else
		return -1;
}

void classTest_addOffest(void * obj, int value)
{
	classTest* cObj = (classTest*)obj;
	if (cObj)
		return cObj->addOffest(value);
}

