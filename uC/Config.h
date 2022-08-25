/*
 * Config.h
 *
 * Created: 23.02.2022 05:05:14
 *  Author: Michael
 */ 


#ifndef CONFIG_H_
#define CONFIG_H_

#include <stdint-gcc.h>

extern uint8_t au8ConfigIOPin[];

/* -------------------- EEPROM MAP -------------------- */
#define CONFIG_SWITCHINGMODULES_EESTART		0			/* Eeprom start address of the retain variables of the relay and powersupply module */
#define CONFIG_TRIGGERMODULE_EESTART		1024		/* Eeprom start address of the retain variables of the trigger module */
#define CONFIG_SWITCHINGMODULES_EESTART		1536		/* Eeprom start address of the configuration variables */


#define CONFIG_RELAYMODULE	/* Use relay module */
//#define CONFIG_POWERSUPPLY	/* Use powersupply module */

#ifdef CONFIG_RELAYMODULE
	#define CONFIG_RELAYMODULE_IOS			8		/* 2,4 or 8 */
	
	#define CONFIG_IO_RELAYMODULE_K1		37 
	#define CONFIG_IO_RELAYMODULE_K2		36
	#if (CONFIG_RELAYMODULE_IOS > 2)
		#define CONFIG_IO_RELAYMODULE_K3	35
		#define CONFIG_IO_RELAYMODULE_K4	34
	#endif	
	#if (CONFIG_RELAYMODULE_IOS == 8)
		#define CONFIG_IO_RELAYMODULE_K5	33
		#define CONFIG_IO_RELAYMODULE_K6	32
		#define CONFIG_IO_RELAYMODULE_K7	31
		#define CONFIG_IO_RELAYMODULE_K8	30
	#endif	/* CONFIG_RELAYMODULE_IOS == 8 */
#else
	#define CONFIG_RELAYMODULE_IOS			0
#endif	/* CONFIG_RELAYMODULE */

#ifdef CONFIG_POWERSUPPLY
	#define CONFIG_POWERSUPPLY_IOS			4		/* 2,4 or 8 */
	
	#define CONFIG_IO_POWERSUPPLY_K1		22
	#define CONFIG_IO_POWERSUPPLY_K2		23
	#if (CONFIG_POWERSUPPLY_IOS > 2)
		#define CONFIG_IO_POWERSUPPLY_K3	24
		#define CONFIG_IO_POWERSUPPLY_K4	25
	#endif
	#if (CONFIG_POWERSUPPLY_IOS == 8)
		#define CONFIG_IO_POWERSUPPLY_K5	26
		#define CONFIG_IO_POWERSUPPLY_K6	27
		#define CONFIG_IO_POWERSUPPLY_K7	28
		#define CONFIG_IO_POWERSUPPLY_K8	29
	#endif	/* CONFIG_POWERSUPPLY_IOS == 8 */
#else
	#define CONFIG_POWERSUPPLY_IOS			0
#endif	/* CONFIG_POWERSUPPLY */

#define CONFIG_TRIGGERMODULE
#ifdef CONFIG_TRIGGERMODULE
	#define CONFIG_TRIGGERMODULE_IOS		1
	
	#define CONFIG_TRIGGERMODULE_IO_0		21			/* INT0 */
	#define CONFIG_TRIGGERMODULE_IO_1		20			/* INT1 */
	#define CONFIG_TRIGGERMODULE_IO_2		2			/* INT4 */
	#define CONFIG_TRIGGERMODULE_IO_3		3			/* INT5 */
	#define CONFIG_TRIGGERMODULE_AIN_0		A0			/* INT0 */
	#define CONFIG_TRIGGERMODULE_AIN_1		A1			/* INT1 */
	#define CONFIG_TRIGGERMODULE_AIN_2		A2			/* INT4 */
	#define CONFIG_TRIGGERMODULE_AIN_3		A3			/* INT5 */
#endif		

/* ------------------------------------------------------------------ */

#define CONFIG_IOS	CONFIG_RELAYMODULE_IOS + CONFIG_POWERSUPPLY_IOS
//#define CONFIG_IOS 1

/* ------------------------------------------------------------------ */
#define CONFIG_SERIAL_DEBUG					Serial
#define CONFIG_SERIAL_COM					Serial3

#define CONFIG_SERIAL_DEBUG_BAUDRATE		115200
#define CONFIG_SERIAL_COM_BAUDRATE			115200

#define CONFIG_SERIAL_DEBUG_PARAM			SERIAL_8N1
#define CONFIG_SERIAL_COM_PARAM				SERIAL_8N1

#define CONFIG_DOCKLIGHT_MODE							/* Docklight dient zur Steuerung */

#define CONFIG_TIMERINTERVAL				20L			/* Timer interval of timer IRQ in ms */

typedef struct  
{
	uint32_t u32InitCode;
	bool bEnableByRetain;							/* Init the anable flags by retain content */
}CONFIG_RETAIN_T;


#endif /* CONFIG_H_ */