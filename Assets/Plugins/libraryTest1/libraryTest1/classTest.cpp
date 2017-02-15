#include "stdafx.h"
#include "classTest.h"


classTest::classTest()
	: m_cpt(0)
{

}


int classTest::getCpt()
{
	return m_cpt;
}

void classTest::addOffest(int value)
{
	m_cpt += value;
}
