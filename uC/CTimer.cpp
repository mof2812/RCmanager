/* 
* CTimer.cpp
*
* Created: 23.02.2022 04:57:27
* Author: Michael
*/


#include "CTimer.h"
#include "CDebug.h"

CTimer Timer;

// default constructor
CTimer::CTimer()
{
} //CTimer

// default destructor
CTimer::~CTimer()
{
} //~CTimer

uint32_t CTimer::Init(void)
{
/* Variable declarations */	
	uint32_t u32RetVal;
	uint8_t i;
	boolean bBreak;
	uint8_t u8Trigger;
	
/* Variable initializations */	
	u32RetVal = 0L;
	bBreak = false;
	
/* Implementation */
	m_u32SystemTimer = millis();
	
	memset(m_au8TriggerMap, 0xFF, sizeof(m_au8TriggerMap));

	for (i=0; (i<CCONFIG_IOS) && (bBreak == false); i++)
	{
		bBreak = (m_aTimerPin[i].Init(i, au8ConfigIOPin[i], &m_u32SystemTimer) != 0L);
		if (bBreak == false)
		{
			u8Trigger = m_aTimerPin[i].GetTrigger();
			/* Set to trigger map */
			m_au8TriggerMap[i] = u8Trigger;
		}
	}
#ifdef CCONFIG_TESTMODE
/* For testing only */
SetTriggerMode(CTIMER_CH_0, CTIMER_TRG_0, true);	
/* End - For testing only */
#endif /* CCONFIG_TESTMODE */

	if (bBreak == true)
	{
		u32RetVal = 1L+i;
	}
	
	/* Set all pin states to 0 */ 
	m_u32PinStates = 0L;
		
	return(u32RetVal);	
}

uint32_t CTimer::GetAsynchronousMode(uint8_t u8TimerChannel, boolean &bSet)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	if (u8TimerChannel < CCONFIG_IOS)
	{
		u32RetVal = m_aTimerPin[u8TimerChannel].GetAsynchronousMode(bSet);
	}
	else
	{
		u32RetVal = 1L;
	}

	return(u32RetVal);
}

uint32_t CTimer::GetCounter(uint8_t u8TimerChannel, uint32_t &u32Counts)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	if (u8TimerChannel < CCONFIG_IOS)
	{
		u32RetVal = m_aTimerPin[u8TimerChannel].GetCounter(u32Counts);
	}
	else
	{
		u32RetVal = 1L;
	}

	return(u32RetVal);
}

uint32_t CTimer::GetCounterAct(uint8_t u8TimerChannel, uint32_t &u32Counts)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	if (u8TimerChannel < CCONFIG_IOS)
	{
		u32RetVal = m_aTimerPin[u8TimerChannel].GetCounterAct(u32Counts);
	}
	else
	{
		u32RetVal = 1L;
	}

	return(u32RetVal);
}

uint32_t CTimer::GetEnable(uint8_t u8TimerChannel, boolean &bEnabled)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	if (u8TimerChannel < CCONFIG_IOS)
	{
		u32RetVal = m_aTimerPin[u8TimerChannel].GetEnable(bEnabled);
	}
	else
	{
		u32RetVal = 1L;
	}
	
	return(u32RetVal);
}

uint32_t CTimer::GetImmediateMode(uint8_t u8TimerChannel, boolean &bSet)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	if (u8TimerChannel < CCONFIG_IOS)
	{
		u32RetVal = m_aTimerPin[u8TimerChannel].GetImmediateMode(bSet);
	}
	else
	{
		u32RetVal = 1L;
	}

	return(u32RetVal);
}

uint32_t CTimer::GetInvertedMode(uint8_t u8TimerChannel, boolean &bSet)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	if (u8TimerChannel < CCONFIG_IOS)
	{
		u32RetVal = m_aTimerPin[u8TimerChannel].GetInvertedMode(bSet);
	}
	else
	{
		u32RetVal = 1L;
	}

	return(u32RetVal);
}

uint32_t CTimer::GetMode(uint8_t u8TimerChannel, CTIMERPIN_MODE_T &Mode)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	if (u8TimerChannel < CCONFIG_IOS)
	{
		u32RetVal = m_aTimerPin[u8TimerChannel].GetMode(Mode);
	}
	else
	{
		u32RetVal = 1L;
	}

	return(u32RetVal);
}

uint32_t CTimer::GetState(uint8_t u8TimerChannel, boolean &bSet)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	if (u8TimerChannel < CCONFIG_IOS)
	{
		u32RetVal = m_aTimerPin[u8TimerChannel].GetState(bSet);
	}
	else
	{
		u32RetVal = 1L;
	}

	return(u32RetVal);
}

uint32_t CTimer::GetTiming(uint8_t u8TimerChannel, CTIMERPIN_TIME_T What, uint32_t &u32Time)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	if (u8TimerChannel < CCONFIG_IOS)
	{
		u32RetVal = m_aTimerPin[u8TimerChannel].GetTiming(What, u32Time);
	}
	else
	{
		u32RetVal = 1L;
	}

	return(u32RetVal);
}

uint32_t CTimer::GetTiming(uint8_t u8TimerChannel, uint32_t &u32TimeOnMS, uint32_t &u32TimeOffMS, uint32_t &u32OffsetMS)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	if (u8TimerChannel < CCONFIG_IOS)
	{
		u32RetVal = m_aTimerPin[u8TimerChannel].GetTiming(u32TimeOnMS, u32TimeOffMS, u32OffsetMS);
	}
	else
	{
		u32RetVal = 1L;
	}

	return(u32RetVal);
}

uint32_t CTimer::GetTriggerMode(uint8_t u8TimerChannel, boolean &bSet)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	if (u8TimerChannel < CCONFIG_IOS)
	{
		u32RetVal = m_aTimerPin[u8TimerChannel].GetTriggerMode(bSet);
	}
	else
	{
		u32RetVal = 1L;
	}

	return(u32RetVal);
}

uint32_t CTimer::GetTriggerMode(uint8_t u8TimerChannel, CTIMERPIN_TRIGGER_T &Trigger, boolean &bSet)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	if (u8TimerChannel < CCONFIG_IOS)
	{
		u32RetVal = m_aTimerPin[u8TimerChannel].GetTriggerMode(Trigger, bSet);
	}
	else
	{
		u32RetVal = 1L;
	}

	return(u32RetVal);
}

uint32_t CTimer::SetAsynchronousMode(uint8_t u8TimerChannel, boolean bSet)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	if (u8TimerChannel < CCONFIG_IOS)
	{
		u32RetVal = m_aTimerPin[u8TimerChannel].SetAsynchronousMode(bSet);
	}

	else
	{
		u32RetVal = 1L;
	}

	return(u32RetVal);
}

uint32_t CTimer::SetCounter(uint8_t u8TimerChannel, uint32_t u32Counts)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	if (u8TimerChannel < CCONFIG_IOS)
	{
		u32RetVal = m_aTimerPin[u8TimerChannel].SetCounter(u32Counts);
	}

	return(u32RetVal);
}

uint32_t CTimer::SetEnable(uint8_t u8TimerChannel, boolean bEnabled)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	if (u8TimerChannel < CCONFIG_IOS)
	{
		u32RetVal = m_aTimerPin[u8TimerChannel].SetEnable(bEnabled);
	}
	
	return(u32RetVal);
}

uint32_t CTimer::SetImmediateMode(uint8_t u8TimerChannel, boolean bSet)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	if (u8TimerChannel < CCONFIG_IOS)
	{
		u32RetVal = m_aTimerPin[u8TimerChannel].SetImmediateMode(bSet);
	}

	else
	{
		u32RetVal = 1L;
	}

	return(u32RetVal);
}

uint32_t CTimer::SetInvertedMode(uint8_t u8TimerChannel, boolean bSet)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	if (u8TimerChannel < CCONFIG_IOS)
	{
		u32RetVal = m_aTimerPin[u8TimerChannel].SetInvertedMode(bSet);
	}

	else
	{
		u32RetVal = 1L;
	}

	return(u32RetVal);
}

uint32_t CTimer::SetMode(uint8_t u8TimerChannel, CTIMERPIN_MODE_T Mode)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	if (u8TimerChannel < CCONFIG_IOS)
	{
		u32RetVal = m_aTimerPin[u8TimerChannel].SetMode(Mode);
	}

	else
	{
		u32RetVal = 1L;
	}

	return(u32RetVal);
}

uint32_t CTimer::SetTiming(uint8_t u8TimerChannel, CTIMERPIN_TIME_T What, uint32_t u32TimeMS)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	if (u8TimerChannel < CCONFIG_IOS)
	{
		u32RetVal = m_aTimerPin[u8TimerChannel].SetTiming(What, u32TimeMS);
	}
	
	else
	{
		u32RetVal = 1L;
	}
	
	return(u32RetVal);
}

uint32_t CTimer::SetTiming(uint8_t u8TimerChannel, uint32_t u32TimeOnMS, uint32_t u32TimeOffMS, uint32_t u32OffsetMS)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	if (u8TimerChannel < CCONFIG_IOS)
	{
		u32RetVal = m_aTimerPin[u8TimerChannel].SetTiming(u32TimeOnMS, u32TimeOffMS, u32OffsetMS);
	}
	else
	{
		u32RetVal = 1L;
	}
	
	return(u32RetVal);
}

uint32_t CTimer::SetTriggerMode(uint8_t u8TimerChannel, boolean bSet)
{
/* Variable declarations */
	uint32_t u32RetVal;

	/* Variable initializations */
	u32RetVal = 0L;

	/* Implementation */
	if (u8TimerChannel < CCONFIG_IOS)
	{
		m_aTimerPin[u8TimerChannel].SetTriggerMode(bSet);
	}
	else
	{
		u32RetVal = 1L;
	}
	
	return(u32RetVal);
}

uint32_t CTimer::SetTriggerMode(uint8_t u8TimerChannel, uint8_t u8TriggerChannel, boolean bSet)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	if (u8TimerChannel < CCONFIG_IOS)
	{
		if(u8TriggerChannel >= CCONFIG_TRIGGERMODULE_IOS)
		{
			u8TriggerChannel = 0xFF;
			bSet = false;
		}
	
		m_aTimerPin[u8TimerChannel].SetTriggerMode(u8TriggerChannel, bSet);
		m_au8TriggerMap[u8TimerChannel] = u8TriggerChannel;
	}
	else
	{
		u32RetVal = 1L;
	}
	
	return(u32RetVal);
}

void CTimer::Trigger(uint8_t u8Channel)
{
/* Variable declarations */
	uint8_t u8TimerChannel;

/* Variable initializations */

/* Implementation */
	for (u8TimerChannel=0; u8TimerChannel < CCONFIG_IOS; u8TimerChannel++)
	{
		if (m_au8TriggerMap[u8TimerChannel] == u8Channel)
		{
//			m_aTimerPin[u8TimerChannel].Trigger();
//			m_aTimerPin[u8TimerChannel].Task();
			m_aTimerPin[u8TimerChannel].SetEnable(true);
		}
	}
}

uint32_t CTimer::Task(void)
{
/* Variable declarations */
	uint32_t u32RetVal;
	uint8_t u8Channel;

/* Variable initializations */
	u32RetVal = 0L;
	m_u32PinStates = 0L;
	bool bState[32];
/* Implementation */
	Debug.ToggleDebugIO(DEBUG_IO_0);
	
	for (u8Channel = 0; u8Channel < CCONFIG_IOS; u8Channel++)
	{
/* Set systemtimer */
		m_u32SystemTimer = millis();
		
		
		//bState[u8Channel] = m_aTimerPin[u8Channel].Task();
		
		m_u32PinStates = (m_aTimerPin[u8Channel].Task() << u8Channel);
	}
	
	return(u32RetVal);
}
