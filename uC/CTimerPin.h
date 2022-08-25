/* 
* CTimerPin.h
*
* Created: 23.02.2022 04:57:27
* Author: Michael
*/


#ifndef __CTimerPin_H__
#define __CTimerPin_H__

#include <Arduino.h>
#include <stdint-gcc.h>

#define CTIMERPIN_EEPROM_INITCODE		0xF0000001

typedef enum
{
	CTIMERPIN_TRIGGER_0 = 0,
	CTIMERPIN_TRIGGER_1,
	CTIMERPIN_TRIGGER_2,
	CTIMERPIN_TRIGGER_3,
	CTIMERPIN_TRIGGER_OFF = 0xFF,
}CTIMERPIN_TRIGGER_T;

typedef enum
{
	CTIMERPIN_MODE_OFF,
	CTIMERPIN_MODE_ON,
	CTIMERPIN_MODE_TOGGLE,
	CTIMERPIN_MODE_IMPULSE
}CTIMERPIN_MODE_T;

typedef enum
{
	CTIMERPIN_STATE_OFF,
	CTIMERPIN_STATE_ON
}CTIMERPIN_STATE_T;

typedef enum
{
	CTIMERPIN_TIME_OFF,
	CTIMERPIN_TIME_ON,
	CTIMERPIN_TIME_OFFSET,
	CTIMERPIN_TIME_UNKNOWN
}CTIMERPIN_TIME_T;

typedef struct
{
	uint32_t u32InitCode;
	CTIMERPIN_MODE_T Mode;
	uint32_t u32TimeOnMS;				/* On time [ms] */
	uint32_t u32TimeOffMS;				/* Off time [ms] */
	uint32_t u32OffsetMS;				/* Offset time [ms] */
	uint32_t u32Counter;				/* Number of clocks in countermode (u32Counter > 0) */
	boolean bInvert;					/* Invert logic */
	boolean bImmediately;				/* Start signal immediately */
	boolean bAsynchronous;				/* Output works asynchronous */
	boolean bTriggerMode;				/* Trigger mode to start action */
	CTIMERPIN_TRIGGER_T Trigger;		/* Trigger channel to trigger the output */
	boolean bEnabled;
	uint8_t Dummy[63];
}CTIMERPIN_RETAIN_T;

typedef struct  
{
	boolean bEnabled;
	boolean bTriggered;
	CTIMERPIN_MODE_T Mode;
	uint32_t u32TargetTime;
	uint32_t u32Counter;				/* Number of clocks in countermode (u32Counter > 0) */
	uint8_t u8Pin;
	uint16_t u16EepromAddress;
	uint8_t u8Index;
	CTIMERPIN_RETAIN_T Retain;
}CTIMERPIN_PARAM_T;

typedef struct
{
	CTIMERPIN_STATE_T State;
}CTIMERPIN_DATA_T;

class CTimerPin
{
//variables
public:
protected:
private:

//functions
public:
	CTimerPin();
	~CTimerPin();
	
	uint32_t ClrCounter(void);
	uint32_t GetAsynchronousMode(boolean &bSet);
	uint32_t GetCounter(uint32_t &u32Counts);
	uint32_t GetCounterAct(uint32_t &u32Counts);
	uint32_t GetImmediateMode(boolean &bSet);
	uint32_t GetInvertedMode(boolean &bSet);
	uint32_t GetEnable(boolean &bSet);
	uint32_t GetMode(CTIMERPIN_MODE_T &Mode);
	uint32_t GetState(boolean &bSet);
	uint32_t GetTiming(CTIMERPIN_TIME_T What, uint32_t &u32Time);
	uint32_t GetTiming(uint32_t &u32TimeOnMS, uint32_t &u32TimeOffMS, uint32_t &u32OffsetMS);
	CTIMERPIN_TRIGGER_T GetTrigger(void);
	uint32_t GetTriggerMode(boolean &bSet);
	uint32_t GetTriggerMode(CTIMERPIN_TRIGGER_T &Trigger, boolean &bSet);
//	uint32_t GetTriggerState(CTIMERPIN_TRIGGER_T Trigger, boolean bSet);
//	uint32_t GetTriggerPin(CTIMERPIN_TRIGGER_T Trigger);
	uint32_t Init(uint8_t u8Index, uint8_t u8Pin, uint32_t* pu32Timer);
	uint32_t SetAsynchronousMode(boolean bSet);
	uint32_t SetCounter(uint32_t u32Counts);
	uint32_t SetEnable(boolean bSet);
	uint32_t SetImmediateMode(boolean bSet);
	uint32_t SetInvertedMode(boolean bSet);
	uint32_t SetMode(CTIMERPIN_MODE_T Mode);
	uint32_t SetTiming(CTIMERPIN_TIME_T What, uint32_t u32TimeMS);
	uint32_t SetTiming(uint32_t u32TimeOnMS, uint32_t u32TimeOffMS, uint32_t u32OffsetMS);
	uint32_t SetTriggerMode(boolean bSet);
	uint32_t SetTriggerMode(CTIMERPIN_TRIGGER_T Trigger, boolean bSet);
	uint32_t Setup(CTIMERPIN_MODE_T Mode, uint32_t u32TimeOn, uint32_t u32TimeOff);
	boolean Task(void);
	void Trigger(void);
protected:
private:
	CTimerPin( const CTimerPin &c );
	CTimerPin& operator=( const CTimerPin &c );
	
	void InitTargetTime(void);
	void SetPin(boolean bSet);
	
	CTIMERPIN_DATA_T m_Data;
	CTIMERPIN_PARAM_T m_Param;
	uint32_t* m_pu32Timer;
}; //CTimerPin

#endif //__CTimerPin_H__
