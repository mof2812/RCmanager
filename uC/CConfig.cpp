/* 
* CConfig.cpp
*
* Created: 07.03.2022 04:59:23
* Author: Michael
*/


#include "CConfig.h"
#include "CDebug.h"
#include <EEPROM.h>

CConfig Config;

uint8_t au8ConfigIOPin[] = {
#ifdef CCONFIG_RELAYMODULE
	CCONFIG_IO_RELAYMODULE_K1,
	CCONFIG_IO_RELAYMODULE_K2,
	#if (CCONFIG_RELAYMODULE_IOS > 2)
		CCONFIG_IO_RELAYMODULE_K3,
		CCONFIG_IO_RELAYMODULE_K4,
	#endif
	#if (CCONFIG_RELAYMODULE_IOS > 4)
		CCONFIG_IO_RELAYMODULE_K5,
		CCONFIG_IO_RELAYMODULE_K6,
		CCONFIG_IO_RELAYMODULE_K7,
		CCONFIG_IO_RELAYMODULE_K8,
	#endif /* CCONFIG_RELAYMODULE_IOS == 8 */
#endif	/* CCONFIG_RELAYMODULE */

#ifdef CCONFIG_POWERSUPPLY
	CCONFIG_IO_POWERSUPPLY_K1,
	CCONFIG_IO_POWERSUPPLY_K2,
	#if (CCONFIG_POWERSUPPLY_IOS > 2)
		CCONFIG_IO_POWERSUPPLY_K3,
		CCONFIG_IO_POWERSUPPLY_K4,
	#endif
	#if (CCONFIG_POWERSUPPLY_IOS > 4)
	CCONFIG_IO_POWERSUPPLY_K5,
	CCONFIG_IO_POWERSUPPLY_K6,
	CCONFIG_IO_POWERSUPPLY_K7,
	CCONFIG_IO_POWERSUPPLY_K8,
	#endif /* CCONFIG_POWERSUPPLY_IOS == 8 */
#endif /* CCONFIG_POWERSUPPLY */
	0xFF
};


// default constructor
CConfig::CConfig()
{
} //CConfig

// default destructor
CConfig::~CConfig()
{
} //~CConfig

uint32_t CConfig::Get(CCONFIG_WHAT_T What, void *pvData)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	switch (What)
	{
	case CCONFIG_WHAT_ENABLE_BY_RETAIN:
		*(boolean*)pvData = m_Param.Retain.bEnableByRetain;
		break;
		
	default:
		u32RetVal = 2L;
		break;
	}

	return(u32RetVal);
}

uint32_t CConfig::Init(void)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	Debug.Log(DEBUG_FLAG_INIT_INFORMATIONS, "Initialize Config data", true);
	
/* Get retains by eeprom */
	m_Param.u16EepromAddress = CCONFIG_CONFIG_EESTART;
	
	EEPROM.get(m_Param.u16EepromAddress, m_Param.Retain);
	
	if (m_Param.Retain.u32InitCode != CCONFIG_EEPROM_INITCODE)
	{
/* Set default values */
		Debug.Log(DEBUG_FLAG_INIT_INFORMATIONS, "Set default values", true);
		
		m_Param.Retain.u32InitCode = CCONFIG_EEPROM_INITCODE;
		m_Param.Retain.bEnableByRetain = false;

/* Save data */
		EEPROM.put(m_Param.u16EepromAddress, m_Param.Retain);
	}

	return(u32RetVal);
}

uint32_t CConfig::Set(CCONFIG_WHAT_T What, uint32_t u32Data)
{
/* Variable declarations */
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	switch (What)
	{
	case CCONFIG_WHAT_ENABLE_BY_RETAIN:
		m_Param.Retain.bEnableByRetain = (u32Data != 0L);
		break;
		
	default:
		u32RetVal = 2L;
		break;	
	}

	if (u32RetVal == 0L)
	{
/* Save data */
		EEPROM.put(m_Param.u16EepromAddress, m_Param.Retain);
	}
	return(u32RetVal);
}
