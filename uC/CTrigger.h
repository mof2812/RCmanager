/* 
* CTrigger.h
*
* Created: 24.02.2022 08:52:18
* Author: Michael
*/


#ifndef __CTRIGGER_H__
#define __CTRIGGER_H__

#include "CConfig.h"
#include <Arduino.h>

#define CTRIGGER_EEPROM_INITCODE		0xE0000001

typedef void (*IRQFunction) (void);

typedef enum
{
	CTRIGGER_MODE_LOW,
	CTRIGGER_MODE_HIGH,
	CTRIGGER_MODE_FALLING,
	CTRIGGER_MODE_RISING,
	CTRIGGER_MODE_LEVEL_DOWN,	
	CTRIGGER_MODE_LEVEL_UP,
	CTRIGGER_MODE_IRQ_DOWN,
	CTRIGGER_MODE_IRQ_UP
}CTRIGGER_MODE_T;

typedef struct
{
	uint32_t u32InitCode;
	boolean abEnable[CCONFIG_TRIGGERMODULE_IOS];
	CTRIGGER_MODE_T aMode[CCONFIG_TRIGGERMODULE_IOS];
	float afTriggerLevel[CCONFIG_TRIGGERMODULE_IOS];
	boolean abReTrigger[CCONFIG_TRIGGERMODULE_IOS];
;
}CTRIGGER_RETAIN_T;

typedef struct  
{
	uint8_t au8Pin[CCONFIG_TRIGGERMODULE_IOS];
	boolean abTriggerReceived[CCONFIG_TRIGGERMODULE_IOS];
	uint16_t au16TriggerLevel[CCONFIG_TRIGGERMODULE_IOS];
//	void (*IRQFct)()[CCONFIG_TRIGGERMODULE_IOS];
	IRQFunction IRQFct[CCONFIG_TRIGGERMODULE_IOS];
	CTRIGGER_RETAIN_T Retain;
}CTRIGGER_PARAM_T;

class CTrigger
{
//variables
public:
protected:
private:

//functions
public:
	CTrigger();
	~CTrigger();
	
	uint32_t EnableRetrigger(uint8_t u8Channel, boolean bEnable); 
	uint32_t EnableTrigger(uint8_t u8Channel, boolean bEnable);
	uint32_t GetMode(uint8_t u8Channel, CTRIGGER_MODE_T &Mode);
	uint32_t GetTriggerLevel(uint8_t u8Channel, float &f32Level);
	uint32_t GetTriggerLevel(uint8_t u8Channel, float &f32Level, uint16_t &u16Level);
	uint32_t Init(void);
	boolean IsEnabled(uint8_t u8Channel) {return(m_Param.Retain.abEnable[u8Channel]);};
	boolean IsRetrigger(uint8_t u8Channel) {return(m_Param.Retain.abReTrigger[u8Channel]);};
	uint32_t ReleaseTrigger(uint8_t u8Channel);
	uint32_t SetTriggerMode(uint8_t u8Channel, CTRIGGER_MODE_T Mode);
	uint32_t SetTriggerLevel(uint8_t u8Channel, float f32Level);
	void Task(void);
protected:
private:
	CTrigger( const CTrigger &c );
	CTrigger& operator=( const CTrigger &c );
	
	uint32_t Init(uint8_t u8Channel);
	void TaskIRQ(uint8_t u8Channel);

	CTRIGGER_PARAM_T m_Param;
}; //CTrigger

extern CTrigger Trigger;

#endif //__CTRIGGER_H__
