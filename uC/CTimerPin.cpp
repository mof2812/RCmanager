/* 
* CTimerPin.cpp
*
* Created: 23.02.2022 04:57:27
* Author: Michael
*/


#include "CTimerPin.h"
#include "CConfig.h"
#include "CDebug.h"
#include <EEPROM.h>

#define TIMERPIN_ON						SetPin(true)
#define TIMERPIN_OFF					SetPin(false)

// default constructor
CTimerPin::CTimerPin()
{
} //CTimerPin

uint32_t CTimerPin::ClrCounter(void)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;
	
/* Implementation */
	m_Param.Retain.u32Counter = 0L;
	m_Param.u32Counter = m_Param.Retain.u32Counter;

#ifdef _DEBUG
	Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, "Wiederholungszähler von Kanal ", m_Param.u8Index + 1, false);
	Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, " geloescht", true);
#endif /* _DEBUG */

/* Save data */
	EEPROM.put(m_Param.u16EepromAddress, m_Param.Retain);

	return(u32RetVal);
}

uint32_t CTimerPin::GetAsynchronousMode(boolean &bSet)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	bSet = m_Param.Retain.bAsynchronous;

	return(u32RetVal);
}

uint32_t CTimerPin::GetCounter(uint32_t &u32Counts)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	u32Counts = m_Param.Retain.u32Counter;

return(u32RetVal);
}

uint32_t CTimerPin::GetCounterAct(uint32_t &u32Counts)
{
	/* Variable declarations */
	uint32_t u32RetVal;

	/* Variable initializations */
	u32RetVal = 0L;

	/* Implementation */
	u32Counts = m_Param.u32Counter;

	return(u32RetVal);
}

uint32_t CTimerPin::GetImmediateMode(boolean &bSet)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	bSet = m_Param.Retain.bImmediately;

	return(u32RetVal);
}

uint32_t CTimerPin::GetInvertedMode(boolean &bSet)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	bSet = m_Param.Retain.bInvert;

	return(u32RetVal);
}

uint32_t CTimerPin::GetEnable(boolean &bEnabled)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	bEnabled = m_Param.bEnabled;

	return(u32RetVal);
}

uint32_t CTimerPin::GetMode(CTIMERPIN_MODE_T &Mode)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	Mode = m_Param.Retain.Mode;

	return(u32RetVal);
}

uint32_t CTimerPin::GetState(boolean &bSet)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	bSet = (m_Data.State != CTIMERPIN_STATE_OFF);

	return(u32RetVal);
}

uint32_t CTimerPin::GetTiming(CTIMERPIN_TIME_T What, uint32_t &u32Time)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	switch (What)
	{
	case CTIMERPIN_TIME_OFF:
		u32Time = m_Param.Retain.u32TimeOffMS;
		break;
		
	case CTIMERPIN_TIME_ON:
		u32Time = m_Param.Retain.u32TimeOnMS;
		break;
		
	case CTIMERPIN_TIME_OFFSET:
		u32Time = m_Param.Retain.u32OffsetMS;
		break;
		
	default:
		u32Time = CTIMERPIN_TIME_UNKNOWN;
		u32RetVal = 1L;	
	}

	return(u32RetVal);
}

uint32_t CTimerPin::GetTiming(uint32_t &u32TimeOnMS, uint32_t &u32TimeOffMS, uint32_t &u32OffsetMS)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	u32TimeOnMS = m_Param.Retain.u32TimeOnMS;
	u32TimeOffMS = m_Param.Retain.u32TimeOffMS;
	u32OffsetMS = m_Param.Retain.u32OffsetMS;

	return(u32RetVal);
}

CTIMERPIN_TRIGGER_T CTimerPin::GetTrigger(void)
{
	return(m_Param.Retain.Trigger);
}


uint32_t CTimerPin::GetTriggerMode(boolean &bSet)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	bSet = m_Param.Retain.bTriggerMode;

	return(u32RetVal);
}

uint32_t CTimerPin::GetTriggerMode(CTIMERPIN_TRIGGER_T &Trigger, boolean &bSet)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	bSet = m_Param.Retain.bTriggerMode;
	Trigger = m_Param.Retain.Trigger;

	return(u32RetVal);
}

uint32_t CTimerPin::Init(uint8_t u8Index, uint8_t u8Pin, uint32_t* pu32Timer)
 {
/* Variable declarations */
	uint32_t u32RetVal;
	 
/* Variable initializations */
	u32RetVal = 0L;
	 
/* Implementation */
	Debug.Log(DEBUG_FLAG_INIT_INFORMATIONS, "Initialize index: ", u8Index, true);
	
/* Get retains by eeprom */
	m_Param.u16EepromAddress = CCONFIG_SWITCHINGMODULES_EESTART + u8Index * sizeof(CTIMERPIN_RETAIN_T);
		
	EEPROM.get(m_Param.u16EepromAddress, m_Param.Retain);
	
/* Initialise parameter */
	m_Param.u8Pin = u8Pin;			/* Set IO pin */
	m_Param.u8Index = u8Index;
	m_Param.Mode = m_Param.Retain.Mode;
	m_Param.u32TargetTime = 0L;
	m_Param.u32Counter = m_Param.Retain.u32Counter;
	m_Param.bTriggered = false;
	m_pu32Timer = pu32Timer;		/* Set systemtimer pointer */
	m_Data.State = CTIMERPIN_STATE_OFF;

	if (m_Param.Retain.u32InitCode != CTIMERPIN_EEPROM_INITCODE)
	{
/* Set default values */
		Debug.Log(DEBUG_FLAG_INIT_INFORMATIONS, "Set default values", true);

		m_Param.Retain.u32InitCode = CTIMERPIN_EEPROM_INITCODE;
		m_Param.Retain.Mode = CTIMERPIN_MODE_OFF;
		m_Param.Retain.u32TimeOffMS = 0L;
		m_Param.Retain.u32TimeOnMS = 0L;
		m_Param.Retain.u32OffsetMS = 0L;
		m_Param.Retain.u32Counter = 0L;
		m_Param.Retain.bInvert = false;
		m_Param.Retain.bImmediately = false;
		m_Param.Retain.bAsynchronous = false;
		m_Param.Retain.bTriggerMode = false;
		m_Param.Retain.Trigger = CTIMERPIN_TRIGGER_OFF;
		
/* Save data */
		EEPROM.put(m_Param.u16EepromAddress, m_Param.Retain);		
	}
	
#ifdef CCONFIG_TESTMODE
/* For testing only */
		SetEnable(true);	/* m_Param.bEnabled = true;	*/
//		SetMode(CTIMERPIN_MODE_TOGGLE);	/* m_Param.Retain.Mode = CTIMERPIN_MODE_TOGGLE; */
		SetMode(CTIMERPIN_MODE_IMPULSE);	/* m_Param.Retain.Mode = CTIMERPIN_MODE_TOGGLE; */
//		SetTiming(CTIMERPIN_TIME_OFF, 1000L);	/* m_Param.Retain.u32TimeOffMS = 1000L; */
//		SetTiming(CTIMERPIN_TIME_ON, 1000L);	/* m_Param.Retain.u32TimeOnMS = 1000L; */
//		SetTiming(CTIMERPIN_TIME_OFFSET, 500L);	/* m_Param.Retain.u32OffsetMS = 500L; */
		SetTiming(1000L, 1000L, 500L);		
		SetInvertedMode(false);	/* m_Param.Retain.bInvert = false; */
		SetImmediateMode(true);	/* m_Param.Retain.bImmediately = true; */
//		SetAsynchronousMode(false);	/* m_Param.Retain.bAsynchronous = false; */
		SetTriggerMode(CTIMERPIN_TRIGGER_0, false); /* m_Param.Retain.Trigger = CTIMERPIN_TRIGGER_0; m_Param.Retain.bTriggerMode; */
//		ClrCounter();
//		SetCounter(5L); /* m_Param.Retain.u32Counter = 5L; */
/* End - For testing only */
#endif /* CCONFIG_TESTMODE */

	m_Param.Mode = m_Param.Retain.Mode;

	InitTargetTime();

	if(m_Param.u8Pin < 54)
	{ 
		Debug.Log(DEBUG_FLAG_INIT_INFORMATIONS, "Initialize Portpin: ", (uint32_t)m_Param.u8Pin, true);
		
		pinMode(m_Param.u8Pin, OUTPUT); 
		digitalWrite(m_Param.u8Pin, LOW);
	}
	else
	{
		u32RetVal = 1L;
	}
	
	if (u32RetVal == 0L)
	{
/* Check for enable */
		if ((Config.IsEnableByRetain() == true)&&(m_Param.Retain.bTriggerMode == false))
		{
			m_Param.bEnabled = m_Param.Retain.bEnabled;
		}
		else
		{
			m_Param.bEnabled = false;		/* Disable output at startup */
		}
	}
		
	
	return(u32RetVal);
 }

void CTimerPin::InitTargetTime(void)	/* Set target time and actual pinstate */
{
	if ((m_Param.Retain.Mode == CTIMERPIN_MODE_OFF) || (m_Param.Retain.Mode == CTIMERPIN_MODE_ON))
	{
		m_Param.u32TargetTime = *m_pu32Timer;
	} 
	else
	{
		if (m_Param.Retain.bAsynchronous == true)
		{
/* Asynchronous mode */
			m_Param.u32TargetTime = *m_pu32Timer - 1;	/* Start immediately */
			m_Data.State = CTIMERPIN_STATE_OFF;
		}
		else
		{
/* Synchronous mode */
			if (m_Param.Retain.bImmediately == true)
			{
				m_Param.u32TargetTime = (*m_pu32Timer - m_Param.Retain.u32OffsetMS) / (m_Param.Retain.u32TimeOnMS + m_Param.Retain.u32TimeOnMS) * (m_Param.Retain.u32TimeOnMS + m_Param.Retain.u32TimeOnMS);
				m_Param.u32TargetTime += m_Param.Retain.u32OffsetMS;
			
				if ((*m_pu32Timer - m_Param.Retain.u32OffsetMS) % (m_Param.Retain.u32TimeOnMS + m_Param.Retain.u32TimeOnMS) < m_Param.Retain.u32TimeOnMS)
				{
					m_Data.State = CTIMERPIN_STATE_OFF;
				} 
				else
				{
					m_Param.u32TargetTime += m_Param.Retain.u32TimeOnMS;
					m_Data.State = CTIMERPIN_STATE_ON;
					if (m_Param.Retain.bTriggerMode == true)
					{
						m_Param.u32Counter++;	/* Because of the first state as 'OFF' state, the counter must be incremented */
					}
				}
			
			
			
			
			}
			else
			{
				m_Param.u32TargetTime = ((*m_pu32Timer - m_Param.Retain.u32OffsetMS - 1) / (m_Param.Retain.u32TimeOnMS + m_Param.Retain.u32TimeOnMS) + 1) * (m_Param.Retain.u32TimeOnMS + m_Param.Retain.u32TimeOnMS);
				m_Param.u32TargetTime += m_Param.Retain.u32OffsetMS;
				m_Data.State = CTIMERPIN_STATE_OFF;
			}
		}
	}
}

uint32_t CTimerPin::SetAsynchronousMode(boolean bSet)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	m_Param.Retain.bAsynchronous = bSet;
	
#ifdef _DEBUG
	if (bSet == true)
	{
		Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, "Asynchrone Betriebsart für Kanal ", m_Param.u8Index + 1, false);
	} 
	else
	{
		Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, "Synchrone Betriebsart für Kanal ", m_Param.u8Index + 1, false);
	}
	Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, " gesetzt", true);
#endif /* _DEBUG */

/* Save data */
	EEPROM.put(m_Param.u16EepromAddress, m_Param.Retain);

	return(u32RetVal);
}

uint32_t CTimerPin::SetCounter(uint32_t u32Counts)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	m_Param.Retain.u32Counter = u32Counts;
	m_Param.u32Counter = u32Counts;

#ifdef _DEBUG
	Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, "Wiederholungszähler von Kanal ", m_Param.u8Index + 1, false);
	Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, " auf ", m_Param.Retain.u32Counter, false);
	Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, " gesetzt", true);
#endif /* _DEBUG */

/* Save data */
	EEPROM.put(m_Param.u16EepromAddress, m_Param.Retain);


	return(u32RetVal);
}

uint32_t CTimerPin::SetEnable(boolean bSet)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	m_Param.bEnabled = bSet;
	
	if ((Config.IsEnableByRetain() == true)&&(m_Param.Retain.bTriggerMode == false))
	{
		m_Param.Retain.bEnabled = bSet;
/* Save data */
		EEPROM.put(m_Param.u16EepromAddress, m_Param.Retain);
	}

	if (m_Param.Retain.Mode == CTIMERPIN_MODE_IMPULSE)
	{
		if (m_Param.Retain.u32Counter != 0L)
		{
			m_Param.u32Counter = m_Param.Retain.u32Counter;
		} 
		else
		{
			m_Param.Mode = CTIMERPIN_MODE_TOGGLE;
		}
	}

	if (bSet == true)
	{
		InitTargetTime();
	}
	else
	{
/* Clear output */
		TIMERPIN_OFF;	
	}
#ifdef _DEBUG	
	Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, "Kanal ", m_Param.u8Index + 1, false);
	if (bSet == true)
	{
		Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, " freigegeben", true);
	} 
	else
	{
		Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, " gesperrt", true);
	}
#endif /* _DEBUG */
	
	return(u32RetVal);
}

uint32_t CTimerPin::SetImmediateMode(boolean bSet)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	m_Param.Retain.bImmediately = bSet;

#ifdef _DEBUG
	Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, "Direktes Setzen des Ausgangs für Kanal ", m_Param.u8Index + 1, false);
	if (bSet == true)
	{
		Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, " gesetzt", true);
	}
	else
	{
		Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, " geloescht", true);
	}
#endif /* _DEBUG */

/* Save data */
	EEPROM.put(m_Param.u16EepromAddress, m_Param.Retain);

	return(u32RetVal);
}

uint32_t CTimerPin::SetInvertedMode(boolean bSet)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	m_Param.Retain.bInvert = bSet;

#ifdef _DEBUG
	Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, "Invertierung für Kanal ", m_Param.u8Index + 1, false);
	if (bSet == true)
	{
		Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, " gesetzt", true);	
	}
	else
	{
		Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, " geloescht", true);
	}
#endif /* _DEBUG */

/* Save data */
	EEPROM.put(m_Param.u16EepromAddress, m_Param.Retain);

	return(u32RetVal);
}

uint32_t CTimerPin::SetMode(CTIMERPIN_MODE_T Mode)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
/* At first clear the output */
	TIMERPIN_OFF;

	switch (Mode)
	{
	case CTIMERPIN_MODE_OFF:
		m_Param.Mode = m_Param.Retain.Mode = Mode;
		m_Data.State = CTIMERPIN_STATE_ON;	/* Will be set to CTIMERPIN_STATE_OFF after the next execution of the task */
		break;
		
	case CTIMERPIN_MODE_ON:
		m_Param.Mode = m_Param.Retain.Mode = Mode;
		m_Data.State = CTIMERPIN_STATE_OFF;	/* Will be set to CTIMERPIN_STATE_ON after the next execution of the task */
		break;
		
	case CTIMERPIN_MODE_TOGGLE:
		m_Param.Mode = m_Param.Retain.Mode = Mode;
		if (m_Param.bEnabled == true)
		{
			InitTargetTime();
		}
		break;
		
	case CTIMERPIN_MODE_IMPULSE:
		m_Param.Mode = m_Param.Retain.Mode = Mode;
		SetCounter(m_Param.Retain.u32Counter);
		SetAsynchronousMode(true);
		if (m_Param.bEnabled == true)
		{
			InitTargetTime();
		}
		break;	

	default:
		u32RetVal = 1L;
		break;
	}
	
	if (u32RetVal == 0L)
	{
/* Save data */
		EEPROM.put(m_Param.u16EepromAddress, m_Param.Retain);
	}
	
#ifdef _DEBUG
	switch (Mode)
	{
	case CTIMERPIN_MODE_OFF:
		Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, "Modus für Kanal ", m_Param.u8Index + 1, false);
		Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, " auf 'Dauer-Aus' gesetzt", true);
		break;
	case CTIMERPIN_MODE_ON:
		Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, "Modus für Kanal ", m_Param.u8Index + 1, false);
		Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, " auf 'Dauer-Ein' gesetzt", true);
		break;
	case CTIMERPIN_MODE_TOGGLE:
		Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, "Modus für Kanal ", m_Param.u8Index + 1, false);
		Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, " auf 'Toggeln' gesetzt", true);
		break;
	case CTIMERPIN_MODE_IMPULSE:
		Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, "Modus für Kanal ", m_Param.u8Index + 1, false);
		Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, " auf 'Impuls' gesetzt", true);
		break;
	default:	
		Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, "Fehler beim Setzen des Modus von Kanal ", m_Param.u8Index + 1, true);
		break;
	}
#endif /* _DEBUG */

return(u32RetVal);
}

uint32_t CTimerPin::SetTiming(CTIMERPIN_TIME_T What, uint32_t u32TimeMS)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	switch (What)
	{
	case CTIMERPIN_TIME_OFF:
		m_Param.Retain.u32TimeOffMS = u32TimeMS;
		break;
	case CTIMERPIN_TIME_ON:
		m_Param.Retain.u32TimeOnMS = u32TimeMS;
		break;
	case CTIMERPIN_TIME_OFFSET:
		m_Param.Retain.u32OffsetMS = u32TimeMS;
		break;
	default:
		u32RetVal = 1L;
		break;	
	}
/* Save data */
	EEPROM.put(m_Param.u16EepromAddress, m_Param.Retain);
	
#ifdef _DEBUG
	if (u32RetVal == 0L)
	{
		switch (What)
		{
		case CTIMERPIN_TIME_OFF:
			Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, "Ausschaltzeit von Kanal ", m_Param.u8Index + 1, false);
			break;
		case CTIMERPIN_TIME_ON:
			Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, "Einschaltzeit von Kanal ", m_Param.u8Index + 1, false);
			break;
		case CTIMERPIN_TIME_OFFSET:
			Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, "Offset von Kanal ", m_Param.u8Index + 1, false);
			break;
		}
		Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, " auf ", u32TimeMS, false);
		Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, "ms gesetzt", true);
	}
	else
	{
		Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, "Fehler beim Setzen einer Zeit von Kanal ", m_Param.u8Index + 1, true);
	}
#endif /* _DEBUG */
	
	return(u32RetVal);
}

uint32_t CTimerPin::SetTiming(uint32_t u32TimeOnMS, uint32_t u32TimeOffMS, uint32_t u32OffsetMS)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	m_Param.Retain.u32TimeOnMS = u32TimeOnMS;
	m_Param.Retain.u32TimeOffMS = u32TimeOffMS;
	m_Param.Retain.u32OffsetMS = u32OffsetMS;

/* Save data */
	EEPROM.put(m_Param.u16EepromAddress, m_Param.Retain);

#ifdef _DEBUG
	Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, "Einschaltzeit von Kanal ", m_Param.u8Index + 1, false);
	Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, " auf ", u32TimeOnMS, false);
	Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, "ms gesetzt", true);
	
	Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, "Ausschaltzeit von Kanal ", m_Param.u8Index + 1, false);
	Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, " auf ", u32TimeOffMS, false);
	Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, "ms gesetzt", true);
	
	Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, "Offset von Kanal ", m_Param.u8Index + 1, false);
	Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, " auf ", u32OffsetMS, false);
	Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, "ms gesetzt", true);
#endif /* _DEBUG */

	return(u32RetVal);
}

uint32_t CTimerPin::SetTriggerMode(boolean bSet)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	m_Param.Retain.bTriggerMode = bSet;
	m_Param.bTriggered = false;	/* Flag zur Anzeige einer erfolgten Triggerung löschen */

#ifdef _DEBUG
	Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, "Aktivierung durch Trigger für Kanal ", m_Param.u8Index + 1, false);

	if (bSet == true)
	{
		Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, " gesetzt", true);
	}
	else
	{
		Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, " geloescht", true);
	}
#endif /* _DEBUG */

/* Save data */
	EEPROM.put(m_Param.u16EepromAddress, m_Param.Retain);

	return(u32RetVal);
}

uint32_t CTimerPin::SetTriggerMode(CTIMERPIN_TRIGGER_T Trigger, boolean bSet)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	if (Trigger == CTIMERPIN_TRIGGER_OFF)
	{
		bSet = false;
	}
	if (bSet == false)
	{
		Trigger = CTIMERPIN_TRIGGER_OFF;
	} 
	m_Param.Retain.bTriggerMode = bSet;
	m_Param.Retain.Trigger = Trigger;
	m_Param.bTriggered = false;	/* Flag zur Anzeige einer erfolgten Triggerung löschen */

#ifdef _DEBUG
	if (Trigger == CTIMERPIN_TRIGGER_OFF)
	{
		Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, "Aktivierung durch Trigger für Kanal ", m_Param.u8Index + 1, false);
	} 
	else
	{
		Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, "Aktivierung durch Trigger(", Trigger, false);
		Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, ") für Kanal ", m_Param.u8Index + 1, false);
	}

	if (bSet == true)
	{
		Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, " gesetzt", true);
	}
	else
	{
		Debug.Log(DEBUG_FLAG_SET_GET_INFORMATIONS, " geloescht", true);
	}
#endif /* _DEBUG */

/* Save data */
	EEPROM.put(m_Param.u16EepromAddress, m_Param.Retain);

	return(u32RetVal);
}

void CTimerPin::SetPin(boolean bSet)
{
	
	if (bSet == true)
	{
#ifdef _MOF		
#ifdef _DEBUG
		Debug.Log(DEBUG_FLAG_IO_INFORMATIONS, "", millis(), false);
		Debug.Log(DEBUG_FLAG_IO_INFORMATIONS, " - ", false);
		Debug.Log(DEBUG_FLAG_IO_INFORMATIONS, "Einschalten von Kanal: ", m_Param.u8Index + 1, true);
#endif /* _DEBUG */
#endif /* _MOF */
		digitalWrite(m_Param.u8Pin, !m_Param.Retain.bInvert);
		m_Data.State = CTIMERPIN_STATE_ON;
		m_Param.u32TargetTime += m_Param.Retain.u32TimeOnMS;
	}
	else
	{
#ifdef _MOF
#ifdef _DEBUG
		Debug.Log(DEBUG_FLAG_IO_INFORMATIONS, "", millis(), false);
		Debug.Log(DEBUG_FLAG_IO_INFORMATIONS, " - ", false);
		Debug.Log(DEBUG_FLAG_IO_INFORMATIONS, "Ausschalten von Kanal: ", m_Param.u8Index + 1, true);
#endif /* _DEBUG */
#endif /* _MOF */
		digitalWrite(m_Param.u8Pin, m_Param.Retain.bInvert);
		m_Data.State = CTIMERPIN_STATE_OFF;
		m_Param.u32TargetTime += m_Param.Retain.u32TimeOffMS;
	}
}

boolean CTimerPin::Task(void)
{
/* Variable declarations */

/* Variable initializations */

/* Implementation */
	if (m_Param.bEnabled == true)
	{
		switch (m_Param.Mode)
		{
		case CTIMERPIN_MODE_OFF:
/* Time do set the output */
			if (m_Data.State == CTIMERPIN_STATE_ON)
			{
				TIMERPIN_OFF;
			}
			break;

		case CTIMERPIN_MODE_ON:
/* Time do set the output */
			if (m_Data.State == CTIMERPIN_STATE_OFF)
			{
				TIMERPIN_ON;
			}
			break;
			
		case CTIMERPIN_MODE_TOGGLE:
			if((m_Param.Retain.u32TimeOffMS > 0L)&&(m_Param.Retain.u32TimeOnMS > 0L))
			{
				if (m_Param.u32TargetTime < *m_pu32Timer + 0.5 * CCONFIG_TIMERINTERVAL)
				{
/* Time do set the output */
					if (m_Data.State == CTIMERPIN_STATE_OFF)
					{
						TIMERPIN_ON;
					}
					else
					{
						TIMERPIN_OFF;
					}
				}
			}
			
			break;
			
		case CTIMERPIN_MODE_IMPULSE:
			if((m_Param.Retain.u32TimeOffMS > 0L)&&(m_Param.Retain.u32TimeOnMS > 0L)&&(m_Param.u32Counter > 0))
			{
				if (m_Param.u32TargetTime < *m_pu32Timer + 0.5 * CCONFIG_TIMERINTERVAL)
				{
					/* Time do set the output */
					if (m_Data.State == CTIMERPIN_STATE_OFF)
					{
						TIMERPIN_ON;
					}
					else
					{
						TIMERPIN_OFF;
						m_Param.u32Counter--;
/*						
						if (--m_Param.u32Counter == 0L)
						{
							m_Param.bEnabled = false;
						}
*/
					}
				}
			}
		
			break;
			
		default:
			break;	
		}
	}
	
	return(digitalRead(m_Param.u8Pin));
}

void CTimerPin::Trigger(void)
{
	if(m_Param.bTriggered == false)
	{
		InitTargetTime();
		m_Param.bTriggered = true;
	}
}

// default destructor
CTimerPin::~CTimerPin()
{
} //~CTimerPin
