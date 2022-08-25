/* 
* CDebug.cpp
*
* Created: 23.02.2022 07:10:50
* Author: Michael
*/


#include "CDebug.h"
#include "CConfig.h"

CDebug Debug;


// default constructor
CDebug::CDebug()
{
	pinMode(LED_BUILTIN, OUTPUT);
	digitalWrite(LED_BUILTIN, LOW);
} //CDebug

// default destructor
CDebug::~CDebug()
{
} //~CDebug

uint32_t CDebug::Init(void)
{
/* Variable declarations */
	 uint32_t u32RetVal;
 
/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
/* Open serial communications and wait for port to open: */
	CCONFIG_SERIAL_DEBUG.begin(CCONFIG_SERIAL_DEBUG_BAUDRATE, CCONFIG_SERIAL_DEBUG_PARAM);
	
	Log(DEBUG_FLAG_INIT_INFORMATIONS, "Debug-Schnittstelle mit ", CCONFIG_SERIAL_DEBUG_BAUDRATE, false);
	Log(DEBUG_FLAG_INIT_INFORMATIONS, "baud initialisiert", true);

#ifdef DEBUG_USE_IOS
/* Initialize debug IOs */
	#ifdef DEBUG_IO_0
		pinMode(DEBUG_IO_0, OUTPUT);
		digitalWrite(DEBUG_IO_0, LOW);
	#endif /* DEBUG_IO_0 */
	#ifdef DEBUG_IO_1
		pinMode(DEBUG_IO_1, OUTPUT);
		digitalWrite(DEBUG_IO_1, LOW);
	#endif /* DEBUG_IO_1 */
	#ifdef DEBUG_IO_2
		pinMode(DEBUG_IO_2, OUTPUT);
		digitalWrite(DEBUG_IO_2, LOW);
	#endif /* DEBUG_IO_2 */
	#ifdef DEBUG_IO_3
		pinMode(DEBUG_IO_3, OUTPUT);
		digitalWrite(DEBUG_IO_3, LOW);
	#endif /* DEBUG_IO_3 */
#endif /* DEBUG_USE_IOS */

	return(u32RetVal);
}

void CDebug::Led(CDEBUG_LED_T What)
{
	switch (What)
	{
	case CDEBUG_LED_OFF:
		digitalWrite(LED_BUILTIN, LOW);
		break;
	case CDEBUG_LED_ON:
		digitalWrite(LED_BUILTIN, HIGH);
		break;
	default:			
		digitalWrite(LED_BUILTIN, LOW);
		break;
	}
}

boolean CDebug::CheckFlag(uint32_t u32FLag)
{
/* Variable declarations */
	boolean bRetVal;
	uint32_t u32Mask;
	
/* Variable initializations */
	bRetVal = false;
	u32Mask = 1L;

/* Implementation */	
	if (DEBUG_FLAG == 0x00000000)
	{
		bRetVal = false;
	}
	else if (DEBUG_FLAG == 0xFFFFFFFF)
	{
		bRetVal = true;
	}
	else
	{
		do 
		{
			if ((u32FLag & u32Mask) == 0L)
			{
				bRetVal = true;
			}
			else
			{
				if (u32Mask == 0x80000000)
				{
					u32Mask = 0L;
				}
				else
				{
					u32Mask <<= 1;
				}
			}
		} while ((bRetVal == false)&&(u32Mask != 0L));
	}
	
	return(bRetVal);
}

CDebug::Log(uint32_t u32Flag, char* szText, boolean bLN)
{
/* Variable declarations */

/* Variable initializations */

/* Implementation */
#ifdef _DEBUG
	if(CheckFlag(u32Flag) != false)
	{
#ifdef DEBUG_ADD_TIMESTAMP
		TimeStamp();
#endif /* DEBUG_ADD_TIMESTAMP */		

		CCONFIG_SERIAL_DEBUG.print(szText);
	
		if (bLN == true)
		{
			CCONFIG_SERIAL_DEBUG.println();
		}
	}
#endif /* _DEBUG */
}

CDebug::Log(uint32_t u32Flag, char* szText, uint32_t u32Value, boolean bLN)
{
	/* Variable declarations */

	/* Variable initializations */

	/* Implementation */
#ifdef _DEBUG
	if(CheckFlag(u32Flag) != false)
	{
#ifdef DEBUG_ADD_TIMESTAMP
		TimeStamp();
#endif /* DEBUG_ADD_TIMESTAMP */

		CCONFIG_SERIAL_DEBUG.print(szText);
		CCONFIG_SERIAL_DEBUG.print(u32Value);
		
		if (bLN == true)
		{
			CCONFIG_SERIAL_DEBUG.println();
		}
	}
#endif /* _DEBUG */	
}

#ifdef DEBUG_USE_IOS
void CDebug::ResetDebugIO(uint8_t u8IO)
{
/* Variable declarations */

/* Variable initializations */

/* Implementation */
	switch (u8IO)
	{
#ifdef DEBUG_IO_0
	case 0:	
	case DEBUG_IO_0:
		digitalWrite(DEBUG_IO_0, LOW);
		break;
#endif /* DEBUG_IO_0 */		

#ifdef DEBUG_IO_1
	case 1:
	case DEBUG_IO_1:
		digitalWrite(DEBUG_IO_1, LOW);
		break;
#endif /* DEBUG_IO_1 */

#ifdef DEBUG_IO_2
	case 2:
	case DEBUG_IO_2:
		digitalWrite(DEBUG_IO_2, LOW);
		break;
#endif /* DEBUG_IO_2 */

#ifdef DEBUG_IO_3
	case 3:
	case DEBUG_IO_3:
		digitalWrite(DEBUG_IO_3, LOW);
		break;
#endif /* DEBUG_IO_3 */
	default:
		break;
	}
}
#endif /* DEBUG_USE_IOS */

#ifdef DEBUG_USE_IOS
void CDebug::SetDebugIO(uint8_t u8IO)
{
/* Variable declarations */

/* Variable initializations */

/* Implementation */
	switch (u8IO)
	{
#ifdef DEBUG_IO_0
	case 0:
	case DEBUG_IO_0:
		digitalWrite(DEBUG_IO_0, HIGH);
		break;
#endif /* DEBUG_IO_0 */

#ifdef DEBUG_IO_1
	case 1:
	case DEBUG_IO_1:
		digitalWrite(DEBUG_IO_1, HIGH);
		break;
#endif /* DEBUG_IO_1 */

#ifdef DEBUG_IO_2
	case 2:
	case DEBUG_IO_2:
		digitalWrite(DEBUG_IO_2, HIGH);
		break;
#endif /* DEBUG_IO_2 */

#ifdef DEBUG_IO_3
	case 3:
	case DEBUG_IO_3:
		digitalWrite(DEBUG_IO_3, HIGH);
		break;
#endif /* DEBUG_IO_3 */
	default:
		break;
	}
}
#endif /* DEBUG_USE_IOS */

CDebug::TimeStamp(void)
{
	CCONFIG_SERIAL_DEBUG.print(millis());
	CCONFIG_SERIAL_DEBUG.print(" - ");
}

#ifdef DEBUG_USE_IOS
void CDebug::ToggleDebugIO(uint8_t u8IO)
{
	/* Variable declarations */

	/* Variable initializations */

	/* Implementation */
	switch (u8IO)
	{
		#ifdef DEBUG_IO_0
		case 0:
		case DEBUG_IO_0:
		digitalWrite(DEBUG_IO_0, !digitalRead(DEBUG_IO_0));
		break;
		#endif /* DEBUG_IO_0 */

		#ifdef DEBUG_IO_1
		case 1:
		case DEBUG_IO_1:
		digitalWrite(DEBUG_IO_1, !digitalRead(DEBUG_IO_1));
		break;
		#endif /* DEBUG_IO_1 */

		#ifdef DEBUG_IO_2
		case 2:
		case DEBUG_IO_2:
		digitalWrite(DEBUG_IO_2, !digitalRead(DEBUG_IO_2));
		break;
		#endif /* DEBUG_IO_2 */

		#ifdef DEBUG_IO_3
		case 3:
		case DEBUG_IO_3:
		digitalWrite(DEBUG_IO_3, !digitalRead(DEBUG_IO_3));
		break;
		#endif /* DEBUG_IO_3 */
		default:
		break;
	}
}
#endif /* DEBUG_USE_IOS */

