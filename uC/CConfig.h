/* 
* CConfig.h
*
* Created: 07.03.2022 04:59:23
* Author: Michael
*/


#ifndef __CCONFIG_H__
#define __CCONFIG_H__

#include <Arduino.h>
#include <stdint-gcc.h>

extern uint8_t au8ConfigIOPin[];

/* -------------------- EEPROM MAP -------------------- */
#define CCONFIG_SWITCHINGMODULES_EESTART		0			/* Eeprom start address of the retain variables of the relay and powersupply module */
#define CCONFIG_TRIGGERMODULE_EESTART			1024		/* Eeprom start address of the retain variables of the trigger module */
#define CCONFIG_CONFIG_EESTART					1536		/* Eeprom start address of the configuration variables */
/* -------------------- END EEPROM MAP -------------------- */
#define CCONFIG_EEPROM_INITCODE					0xE0000001


#define CCONFIG_RELAYMODULE	/* Use relay module */
#define CCONFIG_POWERSUPPLY	/* Use powersupply module */

#ifdef CCONFIG_RELAYMODULE

#if (_DEBUG != 1)
	#define CCONFIG_RELAYMODULE_IOS			8		/* 2,4 or 8 */
#else
	#define CCONFIG_RELAYMODULE_IOS			8		/* 2,4 or 8 */
#endif /* _DEBUG */

	#define CCONFIG_IO_RELAYMODULE_K1		37
	#define CCONFIG_IO_RELAYMODULE_K2		36
	#if (CCONFIG_RELAYMODULE_IOS > 2)
		#define CCONFIG_IO_RELAYMODULE_K3	35
		#define CCONFIG_IO_RELAYMODULE_K4	34
	#endif
	#if (CCONFIG_RELAYMODULE_IOS == 8)
		#define CCONFIG_IO_RELAYMODULE_K5	33
		#define CCONFIG_IO_RELAYMODULE_K6	32
		#define CCONFIG_IO_RELAYMODULE_K7	31
		#define CCONFIG_IO_RELAYMODULE_K8	30
	#endif	/* CCONFIG_RELAYMODULE_IOS == 8 */
#else
	#define CCONFIG_RELAYMODULE_IOS			0
#endif	/* CCONFIG_RELAYMODULE */

#ifdef CCONFIG_POWERSUPPLY

#if (_DEBUG != 1)
	#define CCONFIG_POWERSUPPLY_IOS			8		/* 2,4 or 8 */
#else
	#define CCONFIG_POWERSUPPLY_IOS			1		/* 2,4 or 8 */
#endif /* _DEBUG */

	#define CCONFIG_IO_POWERSUPPLY_K1		22
	#define CCONFIG_IO_POWERSUPPLY_K2		23
	#if (CCONFIG_POWERSUPPLY_IOS > 2)
		#define CCONFIG_IO_POWERSUPPLY_K3	24
		#define CCONFIG_IO_POWERSUPPLY_K4	25
	#endif
	#if (CCONFIG_POWERSUPPLY_IOS == 8)
		#define CCONFIG_IO_POWERSUPPLY_K5	26
		#define CCONFIG_IO_POWERSUPPLY_K6	27
		#define CCONFIG_IO_POWERSUPPLY_K7	28
		#define CCONFIG_IO_POWERSUPPLY_K8	29
	#endif	/* CCONFIG_POWERSUPPLY_IOS == 8 */
#else
	#define CCONFIG_POWERSUPPLY_IOS			0
#endif	/* CCONFIG_POWERSUPPLY */

#define CCONFIG_TRIGGERMODULE
#ifdef CCONFIG_TRIGGERMODULE
#if (_DEBUG != 1)
	#define CCONFIG_TRIGGERMODULE_IOS		4
#else
	#define CCONFIG_TRIGGERMODULE_IOS		1
#endif /* _DEBUG */

	#define CCONFIG_TRIGGERMODULE_IO_0		21			/* INT0 */
	#define CCONFIG_TRIGGERMODULE_IO_1		20			/* INT1 */
	#define CCONFIG_TRIGGERMODULE_IO_2		2			/* INT4 */
	#define CCONFIG_TRIGGERMODULE_IO_3		3			/* INT5 */
	#define CCONFIG_TRIGGERMODULE_AIN_0		A0			/* INT0 */
	#define CCONFIG_TRIGGERMODULE_AIN_1		A1			/* INT1 */
	#define CCONFIG_TRIGGERMODULE_AIN_2		A2			/* INT4 */
	#define CCONFIG_TRIGGERMODULE_AIN_3		A3			/* INT5 */
#endif /* CCONFIG_TRIGGERMODULE */

/* ------------------------------------------------------------------ */

//#ifndef _DEBUG
	#define CCONFIG_IOS	CCONFIG_RELAYMODULE_IOS + CCONFIG_POWERSUPPLY_IOS
/*
#else
	#define CCONFIG_IOS 1
#endif /* _DEBUG */

/* ------------------------------------------------------------------ */
#define CCONFIG_SERIAL_DEBUG					Serial
#define CCONFIG_SERIAL_COM					Serial3

#define CCONFIG_SERIAL_DEBUG_BAUDRATE		115200
#define CCONFIG_SERIAL_COM_BAUDRATE			115200

#define CCONFIG_SERIAL_DEBUG_PARAM			SERIAL_8N1
#define CCONFIG_SERIAL_COM_PARAM				SERIAL_8N1

//#define CCONFIG_DOCKLIGHT_MODE							/* Docklight dient zur Steuerung */

#define CCONFIG_TIMERINTERVAL				20L			/* Timer interval of timer IRQ in ms */

typedef struct
{
	uint32_t u32InitCode;
	boolean bEnableByRetain;							/* Init the anable flags by retain content */
}CCONFIG_RETAIN_T;

typedef struct
{
	uint16_t u16EepromAddress;
	CCONFIG_RETAIN_T Retain;							/* Init the anable flags by retain content */
}CCONFIG_PARAM_T;

typedef enum
{
	CCONFIG_WHAT_ENABLE_BY_RETAIN = 0
}CCONFIG_WHAT_T;

class CConfig
{
//variables
public:
protected:
private:

//functions
public:
	CConfig();
	~CConfig();
	
	uint32_t Get(CCONFIG_WHAT_T What, void *pvData);
	uint32_t Init(void);
	boolean IsEnableByRetain(void){return(m_Param.Retain.bEnableByRetain);};
	uint32_t Set(CCONFIG_WHAT_T What, uint32_t u32Data);
	
protected:
private:
	CConfig( const CConfig &c );
	CConfig& operator=( const CConfig &c );

	CCONFIG_PARAM_T m_Param;
}; //CConfig

extern CConfig Config;

#endif //__CCONFIG_H__
