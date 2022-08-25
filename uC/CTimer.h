/* 
* CTimer.h
*
* Created: 23.02.2022 04:57:27
* Author: Michael
*/
#include "CTimerPin.h"
#include "CConfig.h"
#include <stdint-gcc.h>


#ifndef __CTIMER_H__
#define __CTIMER_H__

typedef enum
{
	CTIMER_CH_0 = 0,
	CTIMER_CH_1,	
	CTIMER_CH_2,
	CTIMER_CH_3,
	CTIMER_CH_4,
	CTIMER_CH_5,
	CTIMER_CH_6,
	CTIMER_CH_7,
	CTIMER_CH_8,
	CTIMER_CH_9,
	CTIMER_CH_10,
	CTIMER_CH_11,
	CTIMER_CH_12,
	CTIMER_CH_13,
	CTIMER_CH_14,
	CTIMER_CH_15
}CTIMER_CH_T;

typedef enum
{
	CTIMER_TRG_0 = 0,
	CTIMER_TRG_1,
	CTIMER_TRG_2,
	CTIMER_TRG_3
}CTIMER_TRG_T;

class CTimer
{
//variables
public:
protected:
private:

//functions
public:
	CTimer();
	~CTimer();
	uint32_t Init(void);
	uint32_t GetAsynchronousMode(uint8_t u8TimerChannel, boolean &bSet);
	uint32_t GetCounter(uint8_t u8TimerChannel, uint32_t &u32Counts);
	uint32_t GetCounterAct(uint8_t u8TimerChannel, uint32_t &u32Counts);
	uint32_t GetEnable(uint8_t u8TimerChannel, boolean &bEnabled);
	uint32_t GetImmediateMode(uint8_t u8TimerChannel, boolean &bSet);
	uint32_t GetInvertedMode(uint8_t u8TimerChannel, boolean &bSet);
	uint32_t GetMode(uint8_t u8TimerChannel, CTIMERPIN_MODE_T &Mode);
	uint32_t GetState(uint8_t u8TimerChannel, boolean &bSet);
	uint32_t GetTiming(uint8_t u8TimerChannel, CTIMERPIN_TIME_T What, uint32_t &u32Time);
	uint32_t GetTiming(uint8_t u8TimerChannel, uint32_t &u32TimeOnMS, uint32_t &u32TimeOffMS, uint32_t &u32OffsetMS);
	uint32_t GetTriggerMode(uint8_t u8TimerChannel, boolean &bSet);
	uint32_t GetTriggerMode(uint8_t u8TimerChannel, CTIMERPIN_TRIGGER_T &Trigger, boolean &bSet);
	uint32_t SetAsynchronousMode(uint8_t u8TimerChannel, boolean bSet);
	uint32_t SetCounter(uint8_t u8TimerChannel, uint32_t u32Counts);
	uint32_t SetEnable(uint8_t u8TimerChannel, boolean bEnabled);
	uint32_t SetImmediateMode(uint8_t u8TimerChannel, boolean bSet);
	uint32_t SetInvertedMode(uint8_t u8TimerChannel, boolean bSet);
	uint32_t SetMode(uint8_t u8TimerChannel, CTIMERPIN_MODE_T Mode);
	uint32_t SetTiming(uint8_t u8TimerChannel, CTIMERPIN_TIME_T What, uint32_t u32TimeMS);
	uint32_t SetTiming(uint8_t u8TimerChannel, uint32_t u32TimeOnMS, uint32_t u32TimeOffMS, uint32_t u32OffsetMS);
	uint32_t SetTriggerMode(uint8_t u8TimerChannel, boolean bSet);
	uint32_t SetTriggerMode(uint8_t u8TimerChannel, uint8_t u8TriggerChannel, boolean bSet);
	void Trigger(uint8_t u8Channel);
	uint32_t Task(void);
protected:
private:
	CTimer( const CTimer &c );
	CTimer& operator=( const CTimer &c );
	
	uint32_t m_u32SystemTimer;
	uint32_t m_u32PinStates;
	CTimerPin m_aTimerPin[CCONFIG_IOS];
	uint8_t m_au8TriggerMap[CCONFIG_IOS];
}; //CTimer

extern CTimer Timer;

#endif //__CTIMER_H__
