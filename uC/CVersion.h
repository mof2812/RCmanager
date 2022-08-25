/* 
* CVersion.h
*
* Created: 28.02.2022 11:56:02
* Author: Michael
*/


#ifndef __CVERSION_H__
#define __CVERSION_H__

#define CVERSION_SW_VERSION			"V1.010 v.07.07.2022"

class CVersion
{
//variables
public:
protected:
private:

//functions
public:
	CVersion();
	~CVersion();
	
	char* GetVersion(void);
protected:
private:
	CVersion( const CVersion &c );
	CVersion& operator=( const CVersion &c );

}; //CVersion

extern CVersion Version;

#endif //__CVERSION_H__
