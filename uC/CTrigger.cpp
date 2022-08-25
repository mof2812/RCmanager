/* 
* CTrigger.cpp
*
* Created: 24.02.2022 08:52:18
* Author: Michael
*/


#include "CTrigger.h"
#include "CTimer.h"
#include "CDebug.h"
#include <EEPROM.h>

void TriggerIRQ_1(void);
void TriggerIRQ_2(void);
void TriggerIRQ_3(void);
void TriggerIRQ_4(void);

CTrigger Trigger;
boolean g_abIRQReceived[4];
boolean g_abIRQdisabled[CCONFIG_TRIGGERMODULE_IOS];

// default constructor
CTrigger::CTrigger()
{
} //CTrigger

// default destructor
CTrigger::~CTrigger()
{
} //~CTrigger

uint32_t CTrigger::EnableRetrigger(uint8_t u8Channel, boolean bEnable)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	if (u8Channel < CCONFIG_TRIGGERMODULE_IOS)
	{
		m_Param.Retain.abReTrigger[u8Channel] = bEnable;
	}
	else
	{
		u32RetVal = 1L; 
	}

/* Save data */
	EEPROM.put(CCONFIG_TRIGGERMODULE_EESTART, m_Param.Retain);
	
	return(u32RetVal);
}

uint32_t CTrigger::EnableTrigger(uint8_t u8Channel, boolean bEnable)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	if (u8Channel < CCONFIG_TRIGGERMODULE_IOS)
	{
		m_Param.Retain.abEnable[u8Channel] = bEnable;
	}
	else
	{
		u32RetVal = 1L;
	}

/* Save data */
	EEPROM.put(CCONFIG_TRIGGERMODULE_EESTART, m_Param.Retain);

	return(u32RetVal);
}

uint32_t CTrigger::GetMode(uint8_t u8Channel, CTRIGGER_MODE_T &Mode)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	if (u8Channel < CCONFIG_TRIGGERMODULE_IOS)
	{
		Mode = m_Param.Retain.aMode[u8Channel];
	}
	else
	{
		u32RetVal = 1L;
	}
	
	return(u32RetVal);
}

uint32_t CTrigger::GetTriggerLevel(uint8_t u8Channel, float &f32Level)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	if (u8Channel < CCONFIG_TRIGGERMODULE_IOS)
	{
		f32Level = m_Param.Retain.afTriggerLevel[u8Channel];
	}
	else
	{
		u32RetVal = 1L;
	}

	return(u32RetVal);
}

uint32_t CTrigger::GetTriggerLevel(uint8_t u8Channel, float &f32Level, uint16_t &u16Level)
{
	/* Variable declarations */
	uint32_t u32RetVal;

	/* Variable initializations */
	u32RetVal = 0L;

	/* Implementation */
	if (u8Channel < CCONFIG_TRIGGERMODULE_IOS)
	{
		f32Level = m_Param.Retain.afTriggerLevel[u8Channel];
		u16Level = m_Param.au16TriggerLevel[u8Channel];
	}
	else
	{
		u32RetVal = 1L;
	}

	return(u32RetVal);
}

uint32_t CTrigger::Init(void)
{
/* Variable declarations */
	uint32_t u32RetVal;
	uint8_t u8Trigger;
	uint16_t u16EepromAddress;

/* Variable initializations */
	u32RetVal = 0L;
	u16EepromAddress = CCONFIG_TRIGGERMODULE_EESTART;

/* Implementation */
/* Get retains by eeprom */
	EEPROM.get(u16EepromAddress, m_Param.Retain);

/* Initialise parameter */	
	for (u8Trigger = 0; u8Trigger < CCONFIG_TRIGGERMODULE_IOS; u8Trigger++)
	{
		m_Param.abTriggerReceived[u8Trigger] = false;
		g_abIRQdisabled[u8Trigger] = false;
	}

	if (m_Param.Retain.u32InitCode != CTRIGGER_EEPROM_INITCODE)
	{
/* Set default values */
		m_Param.Retain.u32InitCode = CTRIGGER_EEPROM_INITCODE;
		for (u8Trigger = 0; u8Trigger < CCONFIG_TRIGGERMODULE_IOS; u8Trigger++)
		{
			m_Param.Retain.aMode[u8Trigger] = CTRIGGER_MODE_FALLING;
			m_Param.Retain.afTriggerLevel[u8Trigger] = 2.5;
			m_Param.Retain.abEnable[u8Trigger] = false;
			m_Param.Retain.abReTrigger[u8Trigger] = false;
		}
/* Save data */		
		EEPROM.put(u16EepromAddress, m_Param.Retain);
	}
	
#ifdef CCONFIG_TESTMODE
/* For testing only */	
	for (u8Trigger = 0; u8Trigger < CCONFIG_TRIGGERMODULE_IOS; u8Trigger++)
	{
		m_Param.Retain.aMode[u8Trigger] = CTRIGGER_MODE_IRQ_DOWN;
		m_Param.Retain.afTriggerLevel[u8Trigger] = 4.0;
		m_Param.Retain.abEnable[u8Trigger] = false;
		m_Param.Retain.abReTrigger[u8Trigger] = true;
	}
/* End - For testing only */
#endif /* CCONFIG_TESTMODE */

	for (u8Trigger = 0; (u8Trigger < CCONFIG_TRIGGERMODULE_IOS) && (u32RetVal == 0L); u8Trigger++)
	{
		u32RetVal = Init(u8Trigger);
		
		if ((m_Param.Retain.aMode[u8Trigger] == CTRIGGER_MODE_LEVEL_UP) || (m_Param.Retain.aMode[u8Trigger] == CTRIGGER_MODE_LEVEL_DOWN))
		{
			if ((m_Param.Retain.afTriggerLevel[u8Trigger] >= 0.0) && (m_Param.Retain.afTriggerLevel[u8Trigger] <= 5.0))
			{
				m_Param.au16TriggerLevel[u8Trigger] = m_Param.Retain.afTriggerLevel[u8Trigger] / 5.0 * 1024;
			} 
			else
			{
				u32RetVal = 2L;
			}
		} 
	}
	return(u32RetVal);
}


uint32_t CTrigger::Init(uint8_t u8Channel)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;
	
/* Implementation */
	if (u8Channel < CCONFIG_TRIGGERMODULE_IOS)
	{
		switch (u8Channel)
		{
		case 0:
			if ((m_Param.Retain.aMode[u8Channel] != CTRIGGER_MODE_LEVEL_DOWN) && (m_Param.Retain.aMode[u8Channel] != CTRIGGER_MODE_LEVEL_UP))
			{
				m_Param.au8Pin[u8Channel] = CCONFIG_TRIGGERMODULE_IO_0;
			}
			else
			{
				m_Param.au8Pin[u8Channel] = CCONFIG_TRIGGERMODULE_AIN_0;
			}
			m_Param.IRQFct[u8Channel] = &TriggerIRQ_1;
			break;	
		
		case 1:
			if ((m_Param.Retain.aMode[u8Channel] != CTRIGGER_MODE_LEVEL_DOWN) && (m_Param.Retain.aMode[u8Channel] != CTRIGGER_MODE_LEVEL_UP))
			{
				m_Param.au8Pin[u8Channel] = CCONFIG_TRIGGERMODULE_IO_1;
			}
			else
			{
				m_Param.au8Pin[u8Channel] = CCONFIG_TRIGGERMODULE_AIN_1;
			}
			m_Param.IRQFct[u8Channel] = &TriggerIRQ_2;
			break;
		
		case 2:
			if ((m_Param.Retain.aMode[u8Channel] != CTRIGGER_MODE_LEVEL_DOWN) && (m_Param.Retain.aMode[u8Channel] != CTRIGGER_MODE_LEVEL_UP))
			{
				m_Param.au8Pin[u8Channel] = CCONFIG_TRIGGERMODULE_IO_2;
			}
			else
			{
				m_Param.au8Pin[u8Channel] = CCONFIG_TRIGGERMODULE_AIN_2;
			}
			m_Param.IRQFct[u8Channel] = &TriggerIRQ_3;
			break;
		
		case 3:
			if ((m_Param.Retain.aMode[u8Channel] != CTRIGGER_MODE_LEVEL_DOWN) && (m_Param.Retain.aMode[u8Channel] != CTRIGGER_MODE_LEVEL_UP))
			{
				m_Param.au8Pin[u8Channel] = CCONFIG_TRIGGERMODULE_IO_3;
			}
			else
			{
				m_Param.au8Pin[u8Channel] = CCONFIG_TRIGGERMODULE_AIN_3;
			}
			m_Param.IRQFct[u8Channel] = &TriggerIRQ_4;
			break;
			
		default:
			u32RetVal = 1L;
			break;	
		}
		
		if ((m_Param.Retain.aMode[u8Channel] != CTRIGGER_MODE_LEVEL_DOWN) && (m_Param.Retain.aMode[u8Channel] != CTRIGGER_MODE_LEVEL_UP))
		{
			pinMode(m_Param.au8Pin[u8Channel], INPUT);
		}
		else
		{
			analogReference(DEFAULT);
		}
		
		switch (m_Param.Retain.aMode[u8Channel])
		{
		case CTRIGGER_MODE_IRQ_DOWN:
			g_abIRQdisabled[u8Channel] = false;
			attachInterrupt(digitalPinToInterrupt(m_Param.au8Pin[u8Channel]), m_Param.IRQFct[u8Channel], FALLING);
			break;
			
		case CTRIGGER_MODE_IRQ_UP:
			g_abIRQdisabled[u8Channel] = false;
			attachInterrupt(digitalPinToInterrupt(m_Param.au8Pin[u8Channel]), m_Param.IRQFct[u8Channel], RISING);
			break;
			
		case CTRIGGER_MODE_LOW:
		case CTRIGGER_MODE_HIGH:
		case CTRIGGER_MODE_LEVEL_DOWN:
		case CTRIGGER_MODE_LEVEL_UP:
			break;
			
		default:
			u32RetVal = 2L;
			break;	
		}
		
#ifdef _DEBUG
		switch (m_Param.Retain.aMode[u8Channel])
		{
		case CTRIGGER_MODE_IRQ_DOWN:
			Debug.Log(DEBUG_FLAG_CTRIGGER_INFORMATIONS, "Trigger ", u8Channel, false);
			Debug.Log(DEBUG_FLAG_CTRIGGER_INFORMATIONS, " für Interrupt-Betrieb (fallende Flanke) initialisiert.", u8Channel, true);
			break;
				
		case CTRIGGER_MODE_IRQ_UP:
			Debug.Log(DEBUG_FLAG_CTRIGGER_INFORMATIONS, "Trigger ", u8Channel, false);
			Debug.Log(DEBUG_FLAG_CTRIGGER_INFORMATIONS, " für Interrupt-Betrieb (steigende Flanke) initialisiert.", u8Channel, true);
			break;
		
		case CTRIGGER_MODE_LOW:
			Debug.Log(DEBUG_FLAG_CTRIGGER_INFORMATIONS, "Trigger ", u8Channel, false);
			Debug.Log(DEBUG_FLAG_CTRIGGER_INFORMATIONS, " für LOW-Pegel-Betrieb initialisiert.", u8Channel, true);
			break;
				
		case CTRIGGER_MODE_HIGH:
			Debug.Log(DEBUG_FLAG_CTRIGGER_INFORMATIONS, "Trigger ", u8Channel, false);
			Debug.Log(DEBUG_FLAG_CTRIGGER_INFORMATIONS, " für HIGH-Pegel-Betrieb initialisiert.", u8Channel, true);
			break;
				
		case CTRIGGER_MODE_LEVEL_DOWN:
			Debug.Log(DEBUG_FLAG_CTRIGGER_INFORMATIONS, "Trigger ", u8Channel, false);
			Debug.Log(DEBUG_FLAG_CTRIGGER_INFORMATIONS, " für Pegel-DOWN-Betrieb (", m_Param.Retain.afTriggerLevel[u8Channel], false);
			Debug.Log(DEBUG_FLAG_CTRIGGER_INFORMATIONS, "V)initialisiert.", false);
			break;
		case CTRIGGER_MODE_LEVEL_UP:
			Debug.Log(DEBUG_FLAG_CTRIGGER_INFORMATIONS, "Trigger ", u8Channel, false);
			Debug.Log(DEBUG_FLAG_CTRIGGER_INFORMATIONS, " für Pegel-UP-Betrieb (", m_Param.Retain.afTriggerLevel[u8Channel], false);
			Debug.Log(DEBUG_FLAG_CTRIGGER_INFORMATIONS, "V)initialisiert.", false);
			break;
		}
#endif /* _DEBUG */		
	}

	return(u32RetVal);
}


uint32_t CTrigger::ReleaseTrigger(uint8_t u8Channel)
{
/* Variable declarations */
	uint32_t u32RetVal;
	
/* Variable initializations */
	u32RetVal = 0L;
	
/* Implementation */
	if (u8Channel < CCONFIG_TRIGGERMODULE_IOS)
	{
		if (m_Param.Retain.abReTrigger[u8Channel] == true)
		{
			if ((m_Param.Retain.aMode[u8Channel] != CTRIGGER_MODE_IRQ_DOWN) && (m_Param.Retain.aMode[u8Channel] != CTRIGGER_MODE_IRQ_UP)) 
			{
				m_Param.abTriggerReceived[u8Channel] = false;
			} 
			else
			{
				g_abIRQReceived[u8Channel] = false;
				g_abIRQdisabled[u8Channel] = false;
			}
		} 
		Debug.Log(DEBUG_FLAG_CTRIGGER_INFORMATIONS, "Trigger ", u8Channel, false);
		Debug.Log(DEBUG_FLAG_CTRIGGER_INFORMATIONS, " retriggert", true);
	} 
	else
	{
		u32RetVal = 1L;
	}
	return(u32RetVal);
}

uint32_t CTrigger::SetTriggerLevel(uint8_t u8Channel, float f32Level)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	if (u8Channel < CCONFIG_TRIGGERMODULE_IOS)
	{
		if ((f32Level >= 0.0) && (f32Level <= 5.0))
		{
			m_Param.Retain.afTriggerLevel[u8Channel] = f32Level;
			m_Param.au16TriggerLevel[u8Channel] = f32Level / 5.0 * 1024;
		}
		else
		{
			u32RetVal = 2L;
		}
	}
	else
	{
		u32RetVal = 1L;
	}

/* Save data */
	EEPROM.put(CCONFIG_TRIGGERMODULE_EESTART, m_Param.Retain);
	
	return(u32RetVal);
}

uint32_t CTrigger::SetTriggerMode(uint8_t u8Channel, CTRIGGER_MODE_T Mode)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	if (u8Channel < CCONFIG_TRIGGERMODULE_IOS)
	{
		m_Param.Retain.aMode[u8Channel] = Mode;
/* Save data */
		EEPROM.put(CCONFIG_TRIGGERMODULE_EESTART, m_Param.Retain);
	}
	else
	{
		u32RetVal = 1L;
	}
	
	return(u32RetVal);
}

void CTrigger::Task(void)
{
/* Variable declarations */
	uint8_t u8Trigger;
	uint8_t u8Pin;
	uint8_t u8State;
	int16_t i16PinState;
	
/* Variable initializations */

/* Implementation */
	for(u8Trigger = 0; u8Trigger < CCONFIG_TRIGGERMODULE_IOS; u8Trigger++)
	{
		if (m_Param.Retain.abEnable[u8Trigger] == false)
		{
			continue;
		}
		
		i16PinState = digitalRead(m_Param.au8Pin[u8Trigger]);
		 
		switch (m_Param.Retain.aMode[u8Trigger])
		{
		case CTRIGGER_MODE_IRQ_DOWN:
		case CTRIGGER_MODE_IRQ_UP:
			TaskIRQ(u8Trigger);
			break;
			
		case CTRIGGER_MODE_LOW:
		
			if (m_Param.abTriggerReceived[u8Trigger] == false)
			{
				if (i16PinState == LOW)
				{
					m_Param.abTriggerReceived[u8Trigger] = true;
					Debug.Log(DEBUG_FLAG_CTRIGGER_INFORMATIONS, "Trigger(LOW) ", u8Trigger, false);
					Debug.Log(DEBUG_FLAG_CTRIGGER_INFORMATIONS, " erfasst", true);
				
					Timer.Trigger(u8Trigger);
				}
			}
			else
			{
				if (i16PinState == HIGH)
				{
					ReleaseTrigger(u8Trigger);
				}
			}
			break;
			
		case CTRIGGER_MODE_HIGH:
		
			if (m_Param.abTriggerReceived[u8Trigger] == false)
			{
				
				if (i16PinState == HIGH)
				{
					m_Param.abTriggerReceived[u8Trigger] = true;
					Debug.Log(DEBUG_FLAG_CTRIGGER_INFORMATIONS, "Trigger(HIGH) ", u8Trigger, false);
					Debug.Log(DEBUG_FLAG_CTRIGGER_INFORMATIONS, " erfasst", true);
					
					Timer.Trigger(u8Trigger);
				}
			}
			else
			{
				if (i16PinState == LOW)
				{
					ReleaseTrigger(u8Trigger);
				}
			}
			break;
		
		case CTRIGGER_MODE_LEVEL_DOWN:
			if (m_Param.abTriggerReceived[u8Trigger] == false)
			{
				if (analogRead(m_Param.au8Pin[u8Trigger]) <= m_Param.au16TriggerLevel[u8Trigger])
				{
					m_Param.abTriggerReceived[u8Trigger] = true;
					Debug.Log(DEBUG_FLAG_CTRIGGER_INFORMATIONS, "Trigger(LEVEL_DW) ", u8Trigger, false);
					Debug.Log(DEBUG_FLAG_CTRIGGER_INFORMATIONS, " erfasst", true);
				
					Timer.Trigger(u8Trigger);
				}
			}
			else
			{
				if (analogRead(m_Param.au8Pin[u8Trigger]) > m_Param.au16TriggerLevel[u8Trigger] + 10)
				{
					ReleaseTrigger(u8Trigger);
				}
			}
			break;
		
		case CTRIGGER_MODE_LEVEL_UP:
		
			if (m_Param.abTriggerReceived[u8Trigger] == false)
			{
				if (analogRead(m_Param.au8Pin[u8Trigger]) >= m_Param.au16TriggerLevel[u8Trigger])
				{
					m_Param.abTriggerReceived[u8Trigger] = true;
					Debug.Log(DEBUG_FLAG_CTRIGGER_INFORMATIONS, "Trigger(LEVEL_UP) ", u8Trigger, false);
					Debug.Log(DEBUG_FLAG_CTRIGGER_INFORMATIONS, " erfasst", true);
					
					Timer.Trigger(u8Trigger);
				}
			}
			else
			{
				if (analogRead(m_Param.au8Pin[u8Trigger]) < m_Param.au16TriggerLevel[u8Trigger] - 10)
				{
					ReleaseTrigger(u8Trigger);
				}
			}
			break;
			
		default:
			break;		
		}
	}
}

void CTrigger::TaskIRQ(uint8_t u8Channel)
{
/* Variable declarations */
	uint8_t u8Trigger;
	uint8_t u8Pin;
	int16_t u16PinState;

/* Variable initializations */

/* Implementation */
	u16PinState = digitalRead(m_Param.au8Pin[u8Channel]);

	if (g_abIRQReceived[u8Channel] == true)
	{
		g_abIRQReceived[u8Channel] = false;
		Debug.Log(DEBUG_FLAG_CTRIGGER_INFORMATIONS, "Trigger ", u8Channel, false);
		Debug.Log(DEBUG_FLAG_CTRIGGER_INFORMATIONS, " erfasst", true);
		Timer.Trigger(u8Channel);
	}
	else
	{
		if ((u16PinState == HIGH) && (m_Param.Retain.abReTrigger[u8Channel] == true) && (g_abIRQdisabled[u8Channel] == true))
		{
			ReleaseTrigger(u8Channel);
		}
	}
}

void TriggerIRQ_1(void)
{
	uint8_t u8Channel = 0;

Debug.ToggleDebugIO(DEBUG_IO_0);
	
	if (Trigger.IsEnabled(u8Channel) == true)
	{
		if (g_abIRQdisabled[u8Channel] == false)
		{
			Timer.Trigger(u8Channel);
			g_abIRQdisabled[u8Channel] = true;
			g_abIRQReceived[u8Channel] = true;
		}
	}
}

void TriggerIRQ_2(void)
{
	uint8_t u8Channel = 1;
	
	if (Trigger.IsEnabled(u8Channel) == true)
	{
		if (g_abIRQdisabled[u8Channel] == false)
		{
			Timer.Trigger(u8Channel);
			g_abIRQdisabled[u8Channel] = true;
			g_abIRQReceived[u8Channel] = true;
		}
	}
}

void TriggerIRQ_3(void)
{
	uint8_t u8Channel = 2;
	
	if (Trigger.IsEnabled(u8Channel) == true)
	{
		if (g_abIRQdisabled[u8Channel] == false)
		{
			Timer.Trigger(u8Channel);
			g_abIRQdisabled[u8Channel] = true;
			g_abIRQReceived[u8Channel] = true;
		}
	}
}

void TriggerIRQ_4(void)
{
	uint8_t u8Channel = 3;
	
	if (Trigger.IsEnabled(u8Channel) == true)
	{
		if (g_abIRQdisabled[u8Channel] == false)
		{
			Timer.Trigger(u8Channel);
			g_abIRQdisabled[u8Channel] = true;
			g_abIRQReceived[u8Channel] = true;
		}
	}
}
