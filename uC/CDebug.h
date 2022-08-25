/* 
* CDebug.h
*
* Created: 23.02.2022 07:10:50
* Author: Michael
*/


#ifndef __CDEBUG_H__
#define __CDEBUG_H__

#include <Arduino.h>
#include <stdint-gcc.h>

typedef enum
{
	CDEBUG_LED_OFF = 0,
	CDEBUG_LED_ON,
	CDEBUG_LED_TOGGLE_500
}CDEBUG_LED_T;

typedef struct
{
	uint32_t u32LedTargetTime;
}CDEBUG_PARAM_T;

//#define DEBUG_ADD_TIMESTAMP	/* Can be done by serial window */

#define DEBUG_FLAG_ALL							0xFFFFFFFF
#define DEBUG_FLAG_INIT_INFORMATIONS			0X00000001
#define DEBUG_FLAG_IO_INFORMATIONS				0X00000002
#define DEBUG_FLAG_SET_GET_INFORMATIONS			0X00000004
#define DEBUG_FLAG_CTRIGGER_INFORMATIONS		0X00000008
#define DEBUG_FLAG_ID_INFORMATIONS				0X00000010

//#define DEBUG_FLAG		DEBUG_FLAG_INIT_INFORMATIONS | DEBUG_FLAG_IO_INFORMATIONS | DEBUG_FLAG_SET_GET_INFORMATIONS
#define DEBUG_FLAG		0//DEBUG_FLAG_ALL

#define DEBUG_USE_IOS
#ifdef DEBUG_USE_IOS
	#define DEBUG_IO_0							53		
	#define DEBUG_IO_1							52
	#define DEBUG_IO_2							51
	#define DEBUG_IO_3							50
#endif /* DEBUG_USE_IOS */

class CDebug
{
//variables
public:
protected:
private:

//functions
public:
	CDebug();
	~CDebug();
	
	uint32_t Init(void);
	void Led(CDEBUG_LED_T What);
	Log(uint32_t u32Flag, char* szText, boolean bLN);
	Log(uint32_t u32Flag, char* szText, uint32_t u32Value, boolean bLN);
#ifdef DEBUG_USE_IOS
	void ResetDebugIO(uint8_t u8IO);
	void SetDebugIO(uint8_t u8IO);
#endif /* DEBUG_USE_IOS */	

	TimeStamp(void);

#ifdef DEBUG_USE_IOS
	void ToggleDebugIO(uint8_t u8IO);
#endif /* DEBUG_USE_IOS */
protected:
private:
	CDebug( const CDebug &c );
	CDebug& operator=( const CDebug &c );
	
	boolean CheckFlag(uint32_t u32FLag);

}; //CDebug

extern CDebug Debug;

#endif //__CDEBUG_H__
