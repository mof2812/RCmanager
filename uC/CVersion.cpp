/* 
* CVersion.cpp
*
* Created: 28.02.2022 11:56:02
* Author: Michael
*/


#include "CVersion.h"

CVersion Version;

// default constructor
CVersion::CVersion()
{
} //CVersion

// default destructor
CVersion::~CVersion()
{
} //~CVersion

char* CVersion::GetVersion(void)
{
	return CVERSION_SW_VERSION;
}
