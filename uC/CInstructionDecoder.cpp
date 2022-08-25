/*************************************************************
project: <type project name here>
author: <type your name here>
description: <type what this file does>
*************************************************************/

#include "CInstructionDecoder.h"
#include "CConfig.h"
#include "CDebug.h"
#include "Help.h"
#include "CTimer.h"
#include "CTrigger.h"
#include "CVersion.h"

//-------------------------------------------------------------------
#define E004		"E004 - Fehler 'Unbekanntes Kommando'"
#define E005		"E005 - Ausfuehrungs-Fehler"
#define E006		"E006 - Parametrierungs-Fehler"
#define E099		"E099 - Sonstiger Fehler"
//-------------------------------------------------------------------

#define MAXCHANNEL(a) (a != 0L ? CCONFIG_POWERSUPPLY_IOS : CCONFIG_RELAYMODULE_IOS)
#define CALCCHANNEL(a,b) (b != 0 ? (uint8_t)a : (uint8_t)(a + CCONFIG_RELAYMODULE_IOS)) 

typedef enum
{
	MODE_YES_NO = 0,
	MODE_X,
	MODE_ON_OFF
}ID_MODE_T;

CInstructionDecoder ID;

CInstructionDecoder::CInstructionDecoder()
{
}	

CInstructionDecoder::~CInstructionDecoder()
{
}

void CInstructionDecoder::BoolToString(boolean bData, char* szString)
{
/* Variable declarations */

/* Variable initializations */

/* Implementation */
	if (bData == false)
	{
		strcpy(szString, "NEIN");
	}
	else
	{
		strcpy(szString, "JA");
	}
}

void CInstructionDecoder::BoolToString(uint8_t u8Mode, boolean bData, char* szString)
{
/* Variable declarations */

/* Variable initializations */

/* Implementation */
	if (bData == false)
	{
		switch (u8Mode)
		{
		case MODE_YES_NO:
			strcpy(szString, "NEIN");
			break;
			
		case MODE_X:
			strcpy(szString, "-");
			break;
			
		case MODE_ON_OFF:
			strcpy(szString, "AUS");
			break;
			
		default:
			strcpy(szString, "FALSE");
			break;				
		}
	}
	else
	{
		switch (u8Mode)
		{
		case MODE_YES_NO:
			strcpy(szString, "JA");
			break;
			
		case MODE_X:
			strcpy(szString, "X");
			break;
			
		case MODE_ON_OFF:
			strcpy(szString, "EIN");
			break;
			
		default:
			strcpy(szString, "TRUE");
			break;
		}
	}
}

void CInstructionDecoder::ChannelToString(uint8_t u8Channel, char* szString)	/* Nur für Netzteil */
{
/* Variable declarations */

/* Variable initializations */

/* Implementation */
	if ((u8Channel >= CCONFIG_RELAYMODULE_IOS)&&(u8Channel < CCONFIG_IOS))
	{
		switch (u8Channel)
		{
		case CCONFIG_RELAYMODULE_IOS:
			strcpy(szString, " 3.3V");
			break;

		case CCONFIG_RELAYMODULE_IOS + 1:
			strcpy(szString, "   5V");
			break;

		case CCONFIG_RELAYMODULE_IOS + 2:
			strcpy(szString, "  12V");
			break;

		case CCONFIG_RELAYMODULE_IOS + 3:
			strcpy(szString, " -12V");
			break;

		default:
			strcpy(szString, " ???");
			break;
	
		}
	} 
	else
	{
		*szString = NULL;
	}
}

void CInstructionDecoder::ClearInstruction(void)
{
	int i;
	
	m_szInstruction[0] = 0;
	for(i=0; i<CINSTRUCTIONDECODER_MAX_PARAMETERS; i++)
	{
		m_szParameter[i][0] = 0;
	}	
}


void CInstructionDecoder::EvaluateFrame(void)
{
	int i16Pointer;
	int i16DataPointer;
	int i16Parameter;
	unsigned char bGetParameter;
	
	bGetParameter = 0;
	i16Parameter = 0;
	i16DataPointer = 0;
	i16Pointer = 0;
	
	while(m_u8RecBuffer[i16DataPointer] != 0)
	{
		if(m_u8RecBuffer[i16DataPointer] == CINSTRUCTIONDECODER_SEPARATOR)
		{
			if (bGetParameter == 0)
			{
				bGetParameter = 1;
				i16Parameter = 0;
			}
			else
			{
				i16Parameter++;
			}
			i16Pointer = 0;				
		}
		else
		{
			if (bGetParameter == 0)
			{
				m_szInstruction[i16Pointer++] = m_u8RecBuffer[i16DataPointer];
				m_szInstruction[i16Pointer] = 0; 
			}			
			else
			{
				if (i16Parameter < CINSTRUCTIONDECODER_MAX_PARAMETERS)
				{	 
					m_szParameter[i16Parameter][i16Pointer++] = m_u8RecBuffer[i16DataPointer];
					m_szParameter[i16Parameter][i16Pointer] = 0;
				}	
			}
		}
		i16DataPointer++;	
	}
		
/* Clear receivebuffer */
	m_u8RecBuffer[0] = 0;

#ifndef MOF
#ifdef CCONFIG_DEBUG_ON
	int i;

	CCONFIG_SERIAL_COM.print("Instruction: ");
	CCONFIG_SERIAL_COM.println((char*)m_szInstruction);
	for(i=0; i<CINSTRUCTIONDECODER_MAX_PARAMETERS; i++)
	{
		CCONFIG_SERIAL_COM.print("Parameter[");
		CCONFIG_SERIAL_COM.print(i+1);
		CCONFIG_SERIAL_COM.print("]: ");
		if(m_szParameter[i][0] == 0)
		{
			CCONFIG_SERIAL_COM.println("---");
		}
		else
		{	
			CCONFIG_SERIAL_COM.println((char*)m_szParameter[i]);
		}	
	} 
#endif
#endif	
}		

void CInstructionDecoder::EvaluateInstruction(void)
{
/* Variable declarations */
	int16_t i16Parameter[CINSTRUCTIONDECODER_MAX_PARAMETERS];
	int32_t i32Parameter[CINSTRUCTIONDECODER_MAX_PARAMETERS];
	uint32_t u32Parameter[CINSTRUCTIONDECODER_MAX_PARAMETERS];
	float f32Parameter[CINSTRUCTIONDECODER_MAX_PARAMETERS];
	uint32_t u32RetVal;
	boolean bE004;
	boolean bOK;
	
/* Variable initializations */
	bE004 = false;
	bOK = true;

/* Implementation */
	switch(m_szInstruction[0])
	{
	case 'C':
/*	
		if (strcmp((char*)m_szInstruction,"XXXX") == 0)
		{
			EvaluateInstruction_XXXX();
		}
		else
		{
			bE004 = true;
		}
*/
		bE004 = true;		
		break;

	case 'D':
		bE004 = true;

		break;
	case 'E':
		bE004 = true;

		break;
	case 'G':
		if (strcmp((char*)m_szInstruction,"GCFG") == 0)
		{
			EvaluateInstruction_GCFG();
		}

		else if (strstr((char*)m_szInstruction,"GPSP") != 0)
		{
			if (strcmp((char*)m_szInstruction,"GPSPAF") == 0)
			{
				EvaluateInstruction_GxxxAF(false);
			}

			else if (strcmp((char*)m_szInstruction,"GPSPC") == 0)
			{
				EvaluateInstruction_GxxxC(false);
			}

			else if (strcmp((char*)m_szInstruction,"GPSPCA") == 0)
			{
				EvaluateInstruction_GxxxCA(false);
			}

			else if (strcmp((char*)m_szInstruction,"GPSPCONFIG") == 0)
			{
				EvaluateInstruction_GxxxCONFIG(false);
			}

			else if (strcmp((char*)m_szInstruction,"GPSPE") == 0)
			{
				EvaluateInstruction_GxxxE(false);
			}

			else if (strcmp((char*)m_szInstruction,"GPSPIF") == 0)
			{
				EvaluateInstruction_GxxxIF(false);
			}

			else if (strcmp((char*)m_szInstruction,"GPSPM") == 0)
			{
				EvaluateInstruction_GxxxM(false);
			}

			else if (strcmp((char*)m_szInstruction,"GPSPNF") == 0)
			{
				EvaluateInstruction_GxxxNF(false);
			}

			else if (strcmp((char*)m_szInstruction,"GPSPT") == 0)
			{
				EvaluateInstruction_GxxxT(false);
			}

			else if (strcmp((char*)m_szInstruction,"GPSPTF") == 0)
			{
				EvaluateInstruction_GxxxTF(false);
			}
		}

		else if (strstr((char*)m_szInstruction,"GSW") != 0)
		{
			if (strcmp((char*)m_szInstruction,"GSWAF") == 0)
			{
				EvaluateInstruction_GxxxAF(true);
			}
		
			else if (strcmp((char*)m_szInstruction,"GSWC") == 0)
			{
				EvaluateInstruction_GxxxC(true);
			}
		
			else if (strcmp((char*)m_szInstruction,"GSWCA") == 0)
			{
				EvaluateInstruction_GxxxCA(true);
			}
		
			else if (strcmp((char*)m_szInstruction,"GSWCONFIG") == 0)
			{
				EvaluateInstruction_GxxxCONFIG(true);
			}
			
			else if (strcmp((char*)m_szInstruction,"GSWE") == 0)
			{
				EvaluateInstruction_GxxxE(true);
			}
		
			else if (strcmp((char*)m_szInstruction,"GSWIF") == 0)
			{
				EvaluateInstruction_GxxxIF(true);
			}
		
			else if (strcmp((char*)m_szInstruction,"GSWM") == 0)
			{
				EvaluateInstruction_GxxxM(true);
			}
		
			else if (strcmp((char*)m_szInstruction,"GSWNF") == 0)
			{
				EvaluateInstruction_GxxxNF(true);
			}
		
			else if (strcmp((char*)m_szInstruction,"GSWT") == 0)
			{
				EvaluateInstruction_GxxxT(true);
			}

			else if (strcmp((char*)m_szInstruction,"GSWTF") == 0)
			{
				EvaluateInstruction_GxxxTF(true);
			}
		}

		else if (strstr((char*)m_szInstruction,"GTRG") != 0)
		{

			if (strcmp((char*)m_szInstruction,"GTRGE") == 0)
			{
				EvaluateInstruction_GTRGE();
			}
			
			else if (strcmp((char*)m_szInstruction,"GTRGL") == 0)
			{
				EvaluateInstruction_GTRGL();
			}

			else if (strcmp((char*)m_szInstruction,"GTRGM") == 0)
			{
				EvaluateInstruction_GTRGM();
				
			}

			else if (strcmp((char*)m_szInstruction,"GTRGR") == 0)
			{
				EvaluateInstruction_GTRGR();
			}
		}
		
		else
		{
			bE004 = true;
		}			

		break;
				
	case 'I':
		if(strcmp((char*)m_szInstruction,"INFO") == 0)			/* Info */
		{
			ClearScreen();
			Info();
		}
		else
		{
			bE004 = true;
		}
		break;

	case 'H':
		if(strcmp((char*)m_szInstruction,"HELP") == 0)			/* Help */
		{
			ClearScreen();
			Help();
		}
		else
		{
			bE004 = true;
		}
		break;

	case 'R':
//		else
		{
			bE004 = true;
		}
		break;

	case 'S':
		if (strcmp((char*)m_szInstruction,"SCFG") == 0)
		{
			EvaluateInstruction_SCFG();
		}
		
		else if (strstr((char*)m_szInstruction,"SPSP") != 0)
		{
			if (strcmp((char*)m_szInstruction,"SPSPAF") == 0)
			{
				EvaluateInstruction_SxxxAF(false);
			}

			else if (strcmp((char*)m_szInstruction,"SPSPC") == 0)
			{
				EvaluateInstruction_SxxxC(false);
			}
			
			else if (strcmp((char*)m_szInstruction,"SPSPCONFIG") == 0)
			{
				EvaluateInstruction_SxxxCONFIG(false);
			}
			
			else if (strcmp((char*)m_szInstruction,"SPSPE") == 0)
			{
				EvaluateInstruction_SxxxE(false);
			}

			else if (strcmp((char*)m_szInstruction,"SPSPIF") == 0)
			{
				EvaluateInstruction_SxxxIF(false);
			}

			else if (strcmp((char*)m_szInstruction,"SPSPM") == 0)
			{
				EvaluateInstruction_SxxxM(false);
			}

			else if (strcmp((char*)m_szInstruction,"SPSPNF") == 0)
			{
				EvaluateInstruction_SxxxNF(false);
			}

			else if (strcmp((char*)m_szInstruction,"SPSPT") == 0)
			{
				EvaluateInstruction_SxxxT(false);
			}

			else if (strcmp((char*)m_szInstruction,"SPSPTF") == 0)
			{
				EvaluateInstruction_SxxxTF(false);
			}
		}
		
		else if (strstr((char*)m_szInstruction,"SSW") != 0)
		{
			if (strcmp((char*)m_szInstruction,"SSWAF") == 0)
			{
				EvaluateInstruction_SxxxAF(true);
			}
			
			else if (strcmp((char*)m_szInstruction,"SSWC") == 0)
			{
				EvaluateInstruction_SxxxC(true);
			}
			
			else if (strcmp((char*)m_szInstruction,"SSWCONFIG") == 0)
			{
				EvaluateInstruction_SxxxCONFIG(true);
			}
			
			else if (strcmp((char*)m_szInstruction,"SSWE") == 0)
			{
				EvaluateInstruction_SxxxE(true);
			}

			else if (strcmp((char*)m_szInstruction,"SSWIF") == 0)
			{
				EvaluateInstruction_SxxxIF(true);
			}
			
			else if (strcmp((char*)m_szInstruction,"SSWM") == 0)
			{
				EvaluateInstruction_SxxxM(true);
			}

			else if (strcmp((char*)m_szInstruction,"SSWNF") == 0)
			{
				EvaluateInstruction_SxxxNF(true);
			}
			
			else if (strcmp((char*)m_szInstruction,"SSWT") == 0)
			{
				EvaluateInstruction_SxxxT(true);
			}

			else if (strcmp((char*)m_szInstruction,"SSWTF") == 0)
			{
				EvaluateInstruction_SxxxTF(true);
			}
		}
		
		else if (strstr((char*)m_szInstruction,"STRG") != 0)
		{
			if (strcmp((char*)m_szInstruction,"STRGE") == 0)
			{
				EvaluateInstruction_STRGE();
			}

			else if (strcmp((char*)m_szInstruction,"STRGL") == 0)
			{
				EvaluateInstruction_STRGL();
			}

			else if (strcmp((char*)m_szInstruction,"STRGM") == 0)
			{
				EvaluateInstruction_STRGM();
			}
		
			else if (strcmp((char*)m_szInstruction,"STRGR") == 0)
			{
				EvaluateInstruction_STRGR();
			}
		}
		
		else
		{
			bE004 = true;
		}
		
		break;
		
	case 'V':
		if(strcmp((char*)m_szInstruction,"VERSION") == 0)			/* Version */
		{
#ifdef CCONFIG_DOCKLIGHT_MODE
		CCONFIG_SERIAL_COM.println();
		CCONFIG_SERIAL_COM.println("Relaiskarten-Steuerung");
		CCONFIG_SERIAL_COM.println("======================");
		CCONFIG_SERIAL_COM.print("Version: ");
		CCONFIG_SERIAL_COM.println(CVERSION_SW_VERSION);
		CCONFIG_SERIAL_COM.println("von Michael Offenbach");
		CCONFIG_SERIAL_COM.println();
#else
			PrintData(Version.GetVersion());
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
		break;
			
	}
	
	if (bE004 == true)	
	{
		PrintError(4);
	}
}	

uint32_t CInstructionDecoder::EvaluateInstruction_GCFG(void)
{
/* Variable declarations */
	uint32_t u32RetVal;
	boolean bData;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	if (strcmp((char*)m_szParameter[0], "ENABLE") == 0)
	{
		if (Config.Get(CCONFIG_WHAT_ENABLE_BY_RETAIN, &bData) == 0)
		{
#ifdef CCONFIG_DOCKLIGHT_MODE
			if (bData == 0)
			{
				CCONFIG_SERIAL_COM.println("Die Schaltkanal-Freigabe muss nach jedem Neustart eingegben werden");
			}
			else
			{
				CCONFIG_SERIAL_COM.println("Die Schaltkanal-Freigabe ist im Eeprom abgelegt");
			}
#else
			PrintData(bData);
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
		else
		{
			u32RetVal = 1L;
			PrintError(5);
		}
	} 
	else
	{
		u32RetVal = 1L;
		PrintError(5);
	}

	return(u32RetVal);
}

uint32_t CInstructionDecoder::EvaluateInstruction_GxxxAF(boolean bSwitch)
{
/* Variable declarations */
	uint32_t u32RetVal;
	uint32_t u32Parameter[CINSTRUCTIONDECODER_MAX_PARAMETERS];
	boolean bData;
	uint32_t u32MaxChannel;

/* Variable initializations */
	u32RetVal = 0L;
	u32MaxChannel = MAXCHANNEL(bSwitch);

/* Implementation */
	u32Parameter[0] = atol((char*)m_szParameter[0]);

	if (u32Parameter[0] < u32MaxChannel)
	{
		u32RetVal = Timer.GetAsynchronousMode(CALCCHANNEL(u32Parameter[0], bSwitch), bData);
		
		if (u32RetVal == 0L)
		{
			if (bData == true)
			{
#ifdef CCONFIG_DOCKLIGHT_MODE
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal ");
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" wird asynchron ausgegeben");
#else
				PrintData("1");
#endif /* CCONFIG_DOCKLIGHT_MODE */
			}
			else
			{
#ifdef CCONFIG_DOCKLIGHT_MODE
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal ");
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" wird synchron ausgegeben");
#else
				PrintData("0");
#endif /* CCONFIG_DOCKLIGHT_MODE */
			}
		}
		else
		{
			u32RetVal = 1L;
			PrintError(5);
		}
	}
	else
	{
		u32RetVal = 2L;
		PrintError(6);
	}

	return(u32RetVal);
}

uint32_t CInstructionDecoder::EvaluateInstruction_GxxxC(boolean bSwitch)
{
/* Variable declarations */
	uint32_t u32RetVal;
	uint32_t u32Parameter[CINSTRUCTIONDECODER_MAX_PARAMETERS];
	uint32_t u32Counts;
	uint32_t u32MaxChannel;

/* Variable initializations */
	u32RetVal = 0L;
	u32MaxChannel = MAXCHANNEL(bSwitch);

/* Implementation */
	u32Parameter[0] = atol((char*)m_szParameter[0]);

	if (u32Parameter[0] < u32MaxChannel)
	{
		u32RetVal = Timer.GetCounter(CALCCHANNEL(u32Parameter[0], bSwitch), u32Counts);
		
		if (u32RetVal == 0L)
		{
#ifdef CCONFIG_DOCKLIGHT_MODE
			CCONFIG_SERIAL_COM.print("Impulsbetrieb von ");
			if (bSwitch == false)
			{
				CCONFIG_SERIAL_COM.print("Netzteil-");
			}
			CCONFIG_SERIAL_COM.print("Schaltkanal ");
			CCONFIG_SERIAL_COM.print(u32Parameter[0]);
			CCONFIG_SERIAL_COM.print(" mit ");
			CCONFIG_SERIAL_COM.print(u32Counts);
			CCONFIG_SERIAL_COM.println(" Impulsen");
#else
			PrintData(u32Counts);
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
		else
		{
			u32RetVal = 1L;
			PrintError(5);
		}
	}
	else
	{
		u32RetVal = 2L;
		PrintError(6);
	}

	return(u32RetVal);
}

uint32_t CInstructionDecoder::EvaluateInstruction_GxxxCA(boolean bSwitch)
{
/* Variable declarations */
	uint32_t u32RetVal;
	uint32_t u32Parameter[CINSTRUCTIONDECODER_MAX_PARAMETERS];
	uint32_t u32Counts;
	uint32_t u32MaxChannel;

/* Variable initializations */
	u32RetVal = 0L;
	u32MaxChannel = MAXCHANNEL(bSwitch);

/* Implementation */
	u32Parameter[0] = atol((char*)m_szParameter[0]);

	if (u32Parameter[0] < u32MaxChannel)
	{
		u32RetVal = Timer.GetCounterAct(CALCCHANNEL(u32Parameter[0], bSwitch), u32Counts);
		
		if (u32RetVal == 0L)
		{
#ifdef CCONFIG_DOCKLIGHT_MODE
			CCONFIG_SERIAL_COM.print("Aktuell noch ");
			CCONFIG_SERIAL_COM.print(u32Counts);
			CCONFIG_SERIAL_COM.print(" Impulse im Impulsbetrieb von ");
			if (bSwitch == false)
			{
				CCONFIG_SERIAL_COM.print("Netzteil-");
			}
			CCONFIG_SERIAL_COM.print("Schaltkanal ");
			CCONFIG_SERIAL_COM.println(u32Parameter[0]);
#else
			PrintData(u32Counts);
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
		else
		{
			u32RetVal = 1L;
			PrintError(5);
		}
	}
	else
	{
		u32RetVal = 2L;
		PrintError(6);
	}

	return(u32RetVal);
}

uint32_t CInstructionDecoder::EvaluateInstruction_GxxxCONFIG(boolean bSwitch)
{
	/* Variable declarations */
	uint32_t u32RetVal;
	uint32_t u32Parameter[CINSTRUCTIONDECODER_MAX_PARAMETERS];
	uint32_t u32MaxChannel;
#ifndef CCONFIG_DOCKLIGHT_MODE
	uint32_t au32Time[3];
	uint32_t u32Counts;
	uint32_t u32CountsActual;
	CTIMERPIN_MODE_T Mode;
	boolean bAsynchronousFlag;
	boolean bEnabledFlag;
	boolean bImmediateFlag;
	boolean bNegationFlag;
	boolean bTriggerFlag;
	char szDataString[128];
#endif

	/* Variable initializations */
	u32RetVal = 0L;
	u32MaxChannel = MAXCHANNEL(bSwitch);

	/* Implementation */
	u32Parameter[0] = atol((char*)m_szParameter[0]);

	if (u32Parameter[0] < u32MaxChannel)
	{
//		u32RetVal = Timer.GetCounter(CALCCHANNEL(u32Parameter[0], bSwitch), u32Counts);
		
		if (u32RetVal == 0L)
		{
#ifdef CCONFIG_DOCKLIGHT_MODE
			if (u32RetVal == 0L)
			{
				EvaluateInstruction_GxxxE(bSwitch);
			}
			if (u32RetVal == 0L)
			{
				u32RetVal = EvaluateInstruction_GxxxM(bSwitch);
			}
			if (u32RetVal == 0L)
			{
				u32RetVal = EvaluateInstruction_GxxxT(bSwitch);
			}
			if (u32RetVal == 0L)
			{
				u32RetVal = EvaluateInstruction_GxxxC(bSwitch);
			}
			if (u32RetVal == 0L)
			{
				u32RetVal = EvaluateInstruction_GxxxCA(bSwitch);
			}
			if (u32RetVal == 0L)
			{
				u32RetVal = EvaluateInstruction_GxxxAF(bSwitch);
			}
			if (u32RetVal == 0L)
			{
				u32RetVal = EvaluateInstruction_GxxxIF(bSwitch);
			}
			if (u32RetVal == 0L)
			{
				EvaluateInstruction_GxxxNF(bSwitch);
			}
			if (u32RetVal == 0L)
			{
				EvaluateInstruction_GxxxTF(bSwitch);
			}
#else
			if (u32RetVal == 0L)
			{
				u32RetVal = Timer.GetEnable(CALCCHANNEL(u32Parameter[0], bSwitch), bEnabledFlag);
			}
			if (u32RetVal == 0L)
			{
				u32RetVal = Timer.GetMode(CALCCHANNEL(u32Parameter[0], bSwitch), Mode);
			}
			if (u32RetVal == 0L)
			{
				u32RetVal = Timer.GetTiming(CALCCHANNEL(u32Parameter[0], bSwitch), CTIMERPIN_TIME_ON, au32Time[0]);
			}
			if (u32RetVal == 0L)
			{
				u32RetVal = Timer.GetTiming(CALCCHANNEL(u32Parameter[0], bSwitch), CTIMERPIN_TIME_OFF, au32Time[1]);
			}
			if (u32RetVal == 0L)
			{
				u32RetVal = Timer.GetTiming(CALCCHANNEL(u32Parameter[0], bSwitch), CTIMERPIN_TIME_OFFSET, au32Time[2]);
			}
			if (u32RetVal == 0L)
			{
				u32RetVal = Timer.GetCounter(CALCCHANNEL(u32Parameter[0], bSwitch), u32Counts);
			}
			if (u32RetVal == 0L)
			{
				u32RetVal = Timer.GetCounterAct(CALCCHANNEL(u32Parameter[0], bSwitch), u32CountsActual);
			}
			if (u32RetVal == 0L)
			{
				u32RetVal = Timer.GetAsynchronousMode(CALCCHANNEL(u32Parameter[0], bSwitch), bAsynchronousFlag);
			}
			if (u32RetVal == 0L)
			{
				u32RetVal = Timer.GetImmediateMode(CALCCHANNEL(u32Parameter[0], bSwitch), bImmediateFlag);
			}
			if (u32RetVal == 0L)
			{
				u32RetVal = Timer.GetInvertedMode(CALCCHANNEL(u32Parameter[0], bSwitch), bNegationFlag);
			}
			if (u32RetVal == 0L)
			{
				u32RetVal = Timer.GetTriggerMode(CALCCHANNEL(u32Parameter[0], bSwitch), bTriggerFlag);
			}
			
			if (u32RetVal == 0L)
			{
				sprintf(szDataString, "%d %d %ld %ld %ld %ld %ld %d %d %d %d", bEnabledFlag, Mode, au32Time[0], au32Time[1], au32Time[2], u32Counts, u32CountsActual, bAsynchronousFlag, bImmediateFlag, bNegationFlag, bTriggerFlag);
			}
			PrintData(szDataString);
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
		else
		{
			u32RetVal = 1L;
			PrintError(5);
		}
	}
	else
	{
		u32RetVal = 2L;
		PrintError(6);
	}

	return(u32RetVal);
}

uint32_t CInstructionDecoder::EvaluateInstruction_GxxxE(boolean bSwitch)
{
/* Variable declarations */
	uint32_t u32RetVal;
	uint32_t u32Parameter[CINSTRUCTIONDECODER_MAX_PARAMETERS];
	boolean bEnabled;
	uint32_t u32MaxChannel;

/* Variable initializations */
	u32RetVal = 0L;
	u32MaxChannel = MAXCHANNEL(bSwitch);

/* Implementation */
	u32Parameter[0] = atol((char*)m_szParameter[0]);

	if (u32Parameter[0] < u32MaxChannel)
	{
		u32RetVal = Timer.GetEnable(CALCCHANNEL(u32Parameter[0], bSwitch), bEnabled);
		
		if (u32RetVal == 0L)
		{
			if (bEnabled == true)
			{
#ifdef CCONFIG_DOCKLIGHT_MODE
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal ");
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" ist freigegeben");
#else
				PrintData("1");
#endif /* CCONFIG_DOCKLIGHT_MODE */
			}
			else
			{
#ifdef CCONFIG_DOCKLIGHT_MODE
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal ");
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" ist gesperrt");
#else
				PrintData("0");
#endif /* CCONFIG_DOCKLIGHT_MODE */
			}
		}
		else
		{
			u32RetVal = 1L;
			PrintError(5);
		}
	}
	else
	{
		u32RetVal = 2L;
		PrintError(6);
	}

	return(u32RetVal);
}


uint32_t CInstructionDecoder::EvaluateInstruction_GxxxIF(boolean bSwitch)
{
/* Variable declarations */
	uint32_t u32RetVal;
	uint32_t u32Parameter[CINSTRUCTIONDECODER_MAX_PARAMETERS];
	boolean bData;
	uint32_t u32MaxChannel;

/* Variable initializations */
	u32RetVal = 0L;
	u32MaxChannel = MAXCHANNEL(bSwitch);

/* Implementation */
	u32Parameter[0] = atol((char*)m_szParameter[0]);

	if (u32Parameter[0] < u32MaxChannel)
	{
		u32RetVal = Timer.GetImmediateMode(CALCCHANNEL(u32Parameter[0], bSwitch), bData);
		
		if (u32RetVal == 0L)
		{
			if (bData == true)
			{
#ifdef CCONFIG_DOCKLIGHT_MODE
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal ");
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" wird unmittelbar ausgegeben");
#else
				PrintData("1");
#endif /* CCONFIG_DOCKLIGHT_MODE */
			}
			else
			{
#ifdef CCONFIG_DOCKLIGHT_MODE
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal ");
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" wird mit Start der ON-Phase ausgegeben");
#else
				PrintData("0");
#endif /* CCONFIG_DOCKLIGHT_MODE */
			}
		}
		else
		{
			u32RetVal = 1L;
			PrintError(5);
		}
	}
	else
	{
		u32RetVal = 2L;
		PrintError(6);
	}

	return(u32RetVal);
}

uint32_t CInstructionDecoder::EvaluateInstruction_GxxxM(boolean bSwitch)
{
/* Variable declarations */
	uint32_t u32RetVal;
	uint32_t u32Parameter[CINSTRUCTIONDECODER_MAX_PARAMETERS];
	CTIMERPIN_MODE_T Mode;
	uint32_t u32MaxChannel;

/* Variable initializations */
	u32RetVal = 0L;
	u32MaxChannel = MAXCHANNEL(bSwitch);

/* Implementation */
	u32Parameter[0] = atol((char*)m_szParameter[0]);

	if (u32Parameter[0] < u32MaxChannel)
	{
		if(Timer.GetMode(CALCCHANNEL(u32Parameter[0], bSwitch), Mode) == 0)
		{
#ifdef CCONFIG_DOCKLIGHT_MODE
			switch (Mode)
			{
			case CTIMERPIN_MODE_OFF:
				CCONFIG_SERIAL_COM.print("Betriebsmodus von ");
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal ");
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" auf 'OFF' gesetzt");
				break;

			case CTIMERPIN_MODE_ON:
				CCONFIG_SERIAL_COM.print("Betriebsmodus von ");
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal ");
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" auf 'ON' gesetzt");
				break;

			case CTIMERPIN_MODE_TOGGLE:
				CCONFIG_SERIAL_COM.print("Betriebsmodus von ");
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal ");
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" auf 'TOGGLE' gesetzt");
				break;

			case CTIMERPIN_MODE_IMPULSE:
				CCONFIG_SERIAL_COM.print("Betriebsmodus von ");
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal ");
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" auf 'IMPULSE' gesetzt");
				break;

			default:
				CCONFIG_SERIAL_COM.println("E006 - Unbekannter Betriebsmodus");
				break;
			}
#else
			PrintData((uint32_t)Mode);
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
		else
		{
			u32RetVal = 1L;
			PrintError(5);
		}
	}
	else
	{
		u32RetVal = 2L;
		PrintError(6);
	}
	
	return(u32RetVal);
}

uint32_t CInstructionDecoder::EvaluateInstruction_GxxxNF(boolean bSwitch)
{
/* Variable declarations */
	uint32_t u32RetVal;
	uint32_t u32Parameter[CINSTRUCTIONDECODER_MAX_PARAMETERS];
	boolean bData;
	uint32_t u32MaxChannel;

/* Variable initializations */
	u32RetVal = 0L;
	u32MaxChannel = MAXCHANNEL(bSwitch);

/* Implementation */
	u32Parameter[0] = atol((char*)m_szParameter[0]);

	if (u32Parameter[0] < u32MaxChannel)
	{
		u32RetVal = Timer.GetInvertedMode(CALCCHANNEL(u32Parameter[0], bSwitch), bData);
		
		if (u32RetVal == 0L)
		{
			if (bData == true)
			{
#ifdef CCONFIG_DOCKLIGHT_MODE
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal ");
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" wird invertiert ausgegeben");
#else
				PrintData("1");
#endif /* CCONFIG_DOCKLIGHT_MODE */
			}
			else
			{
#ifdef CCONFIG_DOCKLIGHT_MODE
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal ");
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" wird nicht invertiert ausgegeben");
#else
				PrintData("0");
#endif /* CCONFIG_DOCKLIGHT_MODE */
			}
		}
		else
		{
			u32RetVal = 1L;
			PrintError(5);
		}
	}
	else
	{
		u32RetVal = 2L;
		PrintError(6);
	}

	return(u32RetVal);
}

uint32_t CInstructionDecoder::EvaluateInstruction_GxxxT(boolean bSwitch)
{
/* Variable declarations */
	uint32_t u32RetVal;
	uint32_t u32Parameter[CINSTRUCTIONDECODER_MAX_PARAMETERS];
	uint32_t u32TimeOnMS;				/* On time [ms] */
	uint32_t u32TimeOffMS;				/* Off time [ms] */
	uint32_t u32OffsetMS;				/* Offset time [ms] */
	uint32_t au32Time[3];
	uint32_t u32MaxChannel;

/* Variable initializations */
	u32RetVal = 0L;
	u32MaxChannel = MAXCHANNEL(bSwitch);

/* Implementation */
	u32Parameter[0] = atol((char*)m_szParameter[0]);

	if (u32Parameter[0] < u32MaxChannel)
	{
		if ((Timer.GetTiming(CALCCHANNEL(u32Parameter[0], bSwitch), CTIMERPIN_TIME_ON, au32Time[0]) == 0) && (Timer.GetTiming(CALCCHANNEL(u32Parameter[0], bSwitch), CTIMERPIN_TIME_OFF, au32Time[1]) == 0) && (Timer.GetTiming(CALCCHANNEL(u32Parameter[0], bSwitch), CTIMERPIN_TIME_OFFSET, au32Time[2]) == 0))
		{
#ifdef CCONFIG_DOCKLIGHT_MODE
			CCONFIG_SERIAL_COM.print("Zeiten von ");
			if (bSwitch == false)
			{
				CCONFIG_SERIAL_COM.print("Netzteil-");
			}
			CCONFIG_SERIAL_COM.print("Schaltkanal ");
			CCONFIG_SERIAL_COM.print(u32Parameter[0]);
			CCONFIG_SERIAL_COM.println(" sind wie folgt eingestellt:");
			CCONFIG_SERIAL_COM.print("- Einschaltzeit:     ");
			CCONFIG_SERIAL_COM.print(au32Time[0]);
			CCONFIG_SERIAL_COM.println("ms");
			CCONFIG_SERIAL_COM.print("- Ausschaltzeit:     ");
			CCONFIG_SERIAL_COM.print(au32Time[1]);
			CCONFIG_SERIAL_COM.println("ms");
			CCONFIG_SERIAL_COM.print("- Verzoegerungszeit: ");
			CCONFIG_SERIAL_COM.print(au32Time[2]);
			CCONFIG_SERIAL_COM.println("ms");
#else
			PrintData(au32Time, 3);
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
		else
		{
			u32RetVal = 1L;
			PrintError(5);
		}
	}
	else
	{
		u32RetVal = 2L;
		PrintError(6);
	}
	return(u32RetVal); 
}

uint32_t CInstructionDecoder::EvaluateInstruction_GxxxTF(boolean bSwitch)
{
/* Variable declarations */
	uint32_t u32RetVal;
	uint32_t u32Parameter[CINSTRUCTIONDECODER_MAX_PARAMETERS];
	boolean bData;
	uint32_t u32MaxChannel;

/* Variable initializations */
	u32RetVal = 0L;
	u32MaxChannel = MAXCHANNEL(bSwitch);

/* Implementation */
	u32Parameter[0] = atol((char*)m_szParameter[0]);

	if (u32Parameter[0] < u32MaxChannel)
	{
		u32RetVal = Timer.GetTriggerMode(CALCCHANNEL(u32Parameter[0], bSwitch), bData);
		
		if (u32RetVal == 0L)
		{
#ifdef CCONFIG_DOCKLIGHT_MODE
			CCONFIG_SERIAL_COM.print("Triggermodus von ");
			if (bSwitch == false)
			{
				CCONFIG_SERIAL_COM.print("Netzteil-");
			}
			CCONFIG_SERIAL_COM.print("Schaltkanal ");
			CCONFIG_SERIAL_COM.print(u32Parameter[0]);
#endif /* CCONFIG_DOCKLIGHT_MODE */

			if (bData == true)
			{
#ifdef CCONFIG_DOCKLIGHT_MODE
				CCONFIG_SERIAL_COM.println(" ist gesetzt");
#else
				PrintData("1");
#endif /* CCONFIG_DOCKLIGHT_MODE */
			}
			else
			{
#ifdef CCONFIG_DOCKLIGHT_MODE
				CCONFIG_SERIAL_COM.println(" ist geloescht");
#else
				PrintData("0");
#endif /* CCONFIG_DOCKLIGHT_MODE */
			}
		}
		else
		{
			u32RetVal = 1L;
			PrintError(5);
		}
	}
	else
	{
		u32RetVal = 2L;
		PrintError(6);
	}

	return(u32RetVal);
}

uint32_t CInstructionDecoder::EvaluateInstruction_GTRGE(void)
{
/* Variable declarations */
	uint32_t u32RetVal;
	int16_t i16Parameter[CINSTRUCTIONDECODER_MAX_PARAMETERS];

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	i16Parameter[0] = atoi((char*)m_szParameter[0]);

	if (i16Parameter[0] < CCONFIG_TRIGGERMODULE_IOS)
	{
		if (Trigger.IsEnabled(i16Parameter[0]) == true)
		{
#ifdef CCONFIG_DOCKLIGHT_MODE
				CCONFIG_SERIAL_COM.print("Triggerkanal ");
				CCONFIG_SERIAL_COM.print(i16Parameter[0]);
				CCONFIG_SERIAL_COM.println(" ist freigegeben");
#else
			PrintData("1");
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
		else
		{
#ifdef CCONFIG_DOCKLIGHT_MODE
				CCONFIG_SERIAL_COM.print("Triggerkanal ");
				CCONFIG_SERIAL_COM.print(i16Parameter[0]);
				CCONFIG_SERIAL_COM.println(" ist gesperrt");
#else
			PrintData("0");
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
	}
	else
	{
		u32RetVal = 2L;
		PrintError(6);
	}

	return(u32RetVal);
}

uint32_t CInstructionDecoder::EvaluateInstruction_GTRGL(void)
{
/* Variable declarations */
	uint32_t u32RetVal;
	int16_t i16Parameter[CINSTRUCTIONDECODER_MAX_PARAMETERS];
	float f32Level;


/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	i16Parameter[0] = atoi((char*)m_szParameter[0]);

	if (i16Parameter[0] < CCONFIG_TRIGGERMODULE_IOS)
	{
		if(Trigger.GetTriggerLevel(i16Parameter[0], f32Level) == 0)
		{
#ifdef CCONFIG_DOCKLIGHT_MODE
			CCONFIG_SERIAL_COM.print("Triggerlevel von Kanal ");
			CCONFIG_SERIAL_COM.print(i16Parameter[0]);
			CCONFIG_SERIAL_COM.print(" ist auf ");
			CCONFIG_SERIAL_COM.print(f32Level);
			CCONFIG_SERIAL_COM.println("V gesetzt");
#else
			PrintData(f32Level);
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
		else
		{
			u32RetVal = 1L;	
			PrintError(5);
		}
	}
	else
	{
		u32RetVal = 2L;
		PrintError(6);
	}
	return(u32RetVal);
}

uint32_t CInstructionDecoder::EvaluateInstruction_GTRGM(void)
{
/* Variable declarations */
	uint32_t u32RetVal;
	int16_t i16Parameter[CINSTRUCTIONDECODER_MAX_PARAMETERS];
	CTRIGGER_MODE_T Mode;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	i16Parameter[0] = atoi((char*)m_szParameter[0]);

	if (i16Parameter[0] < CCONFIG_TRIGGERMODULE_IOS)
	{
		if(Trigger.GetMode(i16Parameter[0], Mode) == 0)
		{
#ifdef CCONFIG_DOCKLIGHT_MODE
			switch (Mode)
			{
			case CTRIGGER_MODE_LOW:
				CCONFIG_SERIAL_COM.print("Triggermodus von Kanal "); 
				CCONFIG_SERIAL_COM.print(i16Parameter[0]);
				CCONFIG_SERIAL_COM.println(" auf 'LOW' gesetzt");
				break;

			case CTRIGGER_MODE_HIGH:
				CCONFIG_SERIAL_COM.print("Triggermodus von Kanal ");
				CCONFIG_SERIAL_COM.print(i16Parameter[0]);
				CCONFIG_SERIAL_COM.println(" auf 'HIGH' gesetzt");
				break;

			case CTRIGGER_MODE_FALLING:
				CCONFIG_SERIAL_COM.print("Triggermodus von Kanal ");
				CCONFIG_SERIAL_COM.print(i16Parameter[0]);
				CCONFIG_SERIAL_COM.println(" auf 'FALLING' gesetzt");
				break;

			case CTRIGGER_MODE_RISING:
				CCONFIG_SERIAL_COM.print("Triggermodus von Kanal ");
				CCONFIG_SERIAL_COM.print(i16Parameter[0]);
				CCONFIG_SERIAL_COM.println(" auf 'RISING' gesetzt");
				break;

			case CTRIGGER_MODE_LEVEL_DOWN:
				CCONFIG_SERIAL_COM.print("Triggermodus von Kanal ");
				CCONFIG_SERIAL_COM.print(i16Parameter[0]);
				CCONFIG_SERIAL_COM.println(" auf 'LEVEL_DOWN' gesetzt");
				break;

			case CTRIGGER_MODE_LEVEL_UP:
				CCONFIG_SERIAL_COM.print("Triggermodus von Kanal ");
				CCONFIG_SERIAL_COM.print(i16Parameter[0]);
				CCONFIG_SERIAL_COM.println(" auf 'LEVEL_UP' gesetzt");
				break;

			case CTRIGGER_MODE_IRQ_DOWN:
				CCONFIG_SERIAL_COM.print("Triggermodus von Kanal ");
				CCONFIG_SERIAL_COM.print(i16Parameter[0]);
				CCONFIG_SERIAL_COM.println(" auf 'IRQ_DOWN' gesetzt");
				break;

			case CTRIGGER_MODE_IRQ_UP:
				CCONFIG_SERIAL_COM.print("Triggermodus von Kanal ");
				CCONFIG_SERIAL_COM.print(i16Parameter[0]);
				CCONFIG_SERIAL_COM.println(" auf 'IRQ_UP' gesetzt");
				break;

			default:
				CCONFIG_SERIAL_COM.println("E006 - Unbekannter Triggermodus");
				break;
			}
#else
			PrintData((uint32_t)Mode);
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
		else
		{
			u32RetVal = 1L;
			PrintError(5);
		}
	}
	else
	{
		u32RetVal = 2L;
		PrintError(6);
	}
	
	return(u32RetVal);
}

uint32_t CInstructionDecoder::EvaluateInstruction_GTRGR(void)
{
/* Variable declarations */
	uint32_t u32RetVal;
	int16_t i16Parameter[CINSTRUCTIONDECODER_MAX_PARAMETERS];

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	i16Parameter[0] = atoi((char*)m_szParameter[0]);

	if (i16Parameter[0] < CCONFIG_TRIGGERMODULE_IOS)
	{
		if (Trigger.IsRetrigger(i16Parameter[0]) == true)
		{
#ifdef CCONFIG_DOCKLIGHT_MODE
			CCONFIG_SERIAL_COM.print("Retrigger von Kanal ");
			CCONFIG_SERIAL_COM.print(i16Parameter[0]);
			CCONFIG_SERIAL_COM.println(" ist freigegeben");
#else
		PrintData("1");
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
		else
		{
#ifdef CCONFIG_DOCKLIGHT_MODE
			CCONFIG_SERIAL_COM.print("Retrigger von Kanal ");
			CCONFIG_SERIAL_COM.print(i16Parameter[0]);
			CCONFIG_SERIAL_COM.println(" ist gesperrt");
#else
			PrintData("0");
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
	}
	else
	{
		u32RetVal = 2L;
		PrintError(6);
	}

	return(u32RetVal);
}

uint32_t CInstructionDecoder::EvaluateInstruction_SCFG(void)
{
/* Variable declarations */
	uint32_t u32RetVal;
	boolean bData;
	uint32_t u32Parameter;


/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	u32Parameter = atol((char*)m_szParameter[1]);

	if (strcmp((char*)m_szParameter[0], "ENABLE") == 0)
	{
		if (Config.Set(CCONFIG_WHAT_ENABLE_BY_RETAIN, (u32Parameter != 0L)) == 0L)
		{
#ifdef CCONFIG_DOCKLIGHT_MODE
			if (u32Parameter == 0L)
			{
				CCONFIG_SERIAL_COM.println("Die Schaltkanal-Freigabe wird nicht im Eeprom abgelegt");
			} 
			else
			{
				CCONFIG_SERIAL_COM.println("Die Schaltkanal-Freigabe wird im Eeprom abgelegt");
			}
#else
			PrintOK();
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
		else
		{
			u32RetVal = 1L;
			PrintError(5);
		}
	}
	else
	{
		u32RetVal = 2L;
		PrintError(6);
	}

	return(u32RetVal);
}

uint32_t CInstructionDecoder::EvaluateInstruction_STRGE(void)
{
/* Variable declarations */
	uint32_t u32RetVal;
	int16_t i16Parameter[CINSTRUCTIONDECODER_MAX_PARAMETERS];

/* Variable initializations */
	u32RetVal = 0L;
	
/* Implementation */
	i16Parameter[0] = atoi((char*)m_szParameter[0]);
	i16Parameter[1] = atoi((char*)m_szParameter[1]);
			
	if (i16Parameter[0] < CCONFIG_TRIGGERMODULE_IOS)
	{
		if (Trigger.EnableTrigger(i16Parameter[0], (boolean)i16Parameter[1]) == 0)
		{
#ifdef CCONFIG_DOCKLIGHT_MODE
			if (i16Parameter[1] != 0)
			{
				CCONFIG_SERIAL_COM.print("Triggerkanal ");
				CCONFIG_SERIAL_COM.print(i16Parameter[0]);
				CCONFIG_SERIAL_COM.println(" freigegeben");
			}
			else
			{
				CCONFIG_SERIAL_COM.print("Triggerkanal ");
				CCONFIG_SERIAL_COM.print(i16Parameter[0]);
				CCONFIG_SERIAL_COM.println(" gesperrt");
			}
#else
			PrintOK();
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
		else
		{
			u32RetVal = 1L;
			PrintError(5);
		}
	}
	else
	{
		u32RetVal = 2L;
		PrintError(6);
	}
	
	return(u32RetVal);
}

uint32_t CInstructionDecoder::EvaluateInstruction_STRGL(void)
{

/* Variable declarations */
	int16_t i16Parameter[CINSTRUCTIONDECODER_MAX_PARAMETERS];
	float f32Parameter[CINSTRUCTIONDECODER_MAX_PARAMETERS];
	uint32_t u32RetVal;

/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	i16Parameter[0] = atoi((char*)m_szParameter[0]);
	f32Parameter[1] = atof((char*)m_szParameter[1]);
			
	if (Trigger.SetTriggerLevel(i16Parameter[0], f32Parameter[1]) == 0)
	{
#ifdef CCONFIG_DOCKLIGHT_MODE
		CCONFIG_SERIAL_COM.print("Triggerlevel von Kanal ");
		CCONFIG_SERIAL_COM.print(i16Parameter[0]);
		CCONFIG_SERIAL_COM.print(" auf ");
		CCONFIG_SERIAL_COM.print(f32Parameter[1]);
		CCONFIG_SERIAL_COM.println("V gesetzt");
#else
		PrintOK();
#endif /* CCONFIG_DOCKLIGHT_MODE */
	}
	else
	{
		u32RetVal = 1L;
		PrintError(5);
	}
	
	return(u32RetVal);
}

uint32_t CInstructionDecoder::EvaluateInstruction_STRGM(void)
{
/* Variable declarations */
	uint32_t u32RetVal;
	int16_t i16Parameter[CINSTRUCTIONDECODER_MAX_PARAMETERS];
	
/* Variable initializations */
	u32RetVal = 0L;

/* Implementation */
	i16Parameter[0] = atoi((char*)m_szParameter[0]);

	if (i16Parameter[0] < CCONFIG_TRIGGERMODULE_IOS)
	{
		if (strcmp((char*)m_szParameter[1], "LOW") == 0)
		{
			u32RetVal = Trigger.SetTriggerMode(i16Parameter[0], CTRIGGER_MODE_LOW);
		
#ifdef CCONFIG_DOCKLIGHT_MODE
			if (u32RetVal == 0L)
			{
				CCONFIG_SERIAL_COM.print("Triggermodus von Kanal ");
				CCONFIG_SERIAL_COM.print(i16Parameter[0]);
				CCONFIG_SERIAL_COM.println(" auf 'LOW' gesetzt");
			}
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}

		else if (strcmp((char*)m_szParameter[1], "HIGH") == 0)
		{
			u32RetVal = Trigger.SetTriggerMode(i16Parameter[0], CTRIGGER_MODE_HIGH);

#ifdef CCONFIG_DOCKLIGHT_MODE
			if (u32RetVal == 0L)
			{
				CCONFIG_SERIAL_COM.print("Triggermodus von Kanal ");
				CCONFIG_SERIAL_COM.print(i16Parameter[0]);
				CCONFIG_SERIAL_COM.println(" auf 'HIGH' gesetzt");
			}
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
		
		else if (strcmp((char*)m_szParameter[1], "FALLING") == 0)
		{
			u32RetVal = Trigger.SetTriggerMode(i16Parameter[0], CTRIGGER_MODE_FALLING);

#ifdef CCONFIG_DOCKLIGHT_MODE
			if (u32RetVal == 0L)
			{
				CCONFIG_SERIAL_COM.print("Triggermodus von Kanal ");
				CCONFIG_SERIAL_COM.print(i16Parameter[0]);
				CCONFIG_SERIAL_COM.println(" auf 'FALLING' gesetzt");
			}
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}

		else if (strcmp((char*)m_szParameter[1], "RISING") == 0)
		{
			u32RetVal = Trigger.SetTriggerMode(i16Parameter[0], CTRIGGER_MODE_RISING);

#ifdef CCONFIG_DOCKLIGHT_MODE
			if (u32RetVal == 0L)
			{
				CCONFIG_SERIAL_COM.print("Triggermodus von Kanal ");
				CCONFIG_SERIAL_COM.print(i16Parameter[0]);
				CCONFIG_SERIAL_COM.println(" auf 'RISING' gesetzt");
			}
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}

		else if (strcmp((char*)m_szParameter[1], "LEVEL_DOWN") == 0)
		{
			u32RetVal = Trigger.SetTriggerMode(i16Parameter[0], CTRIGGER_MODE_LEVEL_DOWN);

#ifdef CCONFIG_DOCKLIGHT_MODE
			if (u32RetVal == 0L)
			{
				CCONFIG_SERIAL_COM.print("Triggermodus von Kanal ");
				CCONFIG_SERIAL_COM.print(i16Parameter[0]);
				CCONFIG_SERIAL_COM.println(" auf 'LEVEL_DOWN' gesetzt");
			}
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}

		else if (strcmp((char*)m_szParameter[1], "LEVEL_UP") == 0)
		{
			u32RetVal = Trigger.SetTriggerMode(i16Parameter[0], CTRIGGER_MODE_LEVEL_UP);

#ifdef CCONFIG_DOCKLIGHT_MODE
			if (u32RetVal == 0L)
			{
				CCONFIG_SERIAL_COM.print("Triggermodus von Kanal ");
				CCONFIG_SERIAL_COM.print(i16Parameter[0]);
				CCONFIG_SERIAL_COM.println(" auf 'LEVEL_UP' gesetzt");
			}
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}

		else if (strcmp((char*)m_szParameter[1], "IRQ_DOWN") == 0)
		{
			u32RetVal = Trigger.SetTriggerMode(i16Parameter[0], CTRIGGER_MODE_IRQ_DOWN);

#ifdef CCONFIG_DOCKLIGHT_MODE
			if (u32RetVal == 0L)
			{
				CCONFIG_SERIAL_COM.print("Triggermodus von Kanal ");
				CCONFIG_SERIAL_COM.print(i16Parameter[0]);
				CCONFIG_SERIAL_COM.println(" auf 'IRQ_DOWN' gesetzt");
			}
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
		
		else if (strcmp((char*)m_szParameter[1], "IRQ_UP") == 0)
		{
			u32RetVal = Trigger.SetTriggerMode(i16Parameter[0], CTRIGGER_MODE_IRQ_UP);

#ifdef CCONFIG_DOCKLIGHT_MODE
			if (u32RetVal == 0L)
			{
				CCONFIG_SERIAL_COM.print("Triggermodus von Kanal ");
				CCONFIG_SERIAL_COM.print(i16Parameter[0]);
				CCONFIG_SERIAL_COM.println(" auf 'IRQ_UP' gesetzt");
			}
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
		
		else
		{
			/* Error - no valide mode*/
			u32RetVal = 1L;
		}
	} 
	else
	{
		u32RetVal = 6L;
	}

/* Examination of u32RetVal */
	if (u32RetVal != 0L)
	{
		switch (u32RetVal)
		{
		case 6:
			PrintError(6);
			break;
		
		default:
			PrintError(5);
			break;
		}
	}
#ifndef CCONFIG_DOCKLIGHT_MODE
	else
	{
		PrintOK();
	}
#endif /* CCONFIG_DOCKLIGHT_MODE */

	return(u32RetVal);
}

uint32_t CInstructionDecoder::EvaluateInstruction_STRGR(void)
{
	/* Variable declarations */
	int16_t i16Parameter[CINSTRUCTIONDECODER_MAX_PARAMETERS];
	uint32_t u32RetVal;

	/* Variable initializations */
	u32RetVal = 0L;
	
	/* Implementation */
	i16Parameter[0] = atoi((char*)m_szParameter[0]);
	i16Parameter[1] = atoi((char*)m_szParameter[1]);
	
	if (i16Parameter[0] < CCONFIG_TRIGGERMODULE_IOS)
	{
		if (Trigger.EnableRetrigger(i16Parameter[0], (boolean)i16Parameter[1]) == 0)
		{
#ifdef CCONFIG_DOCKLIGHT_MODE
			if (i16Parameter[1] == 0)
			{
				CCONFIG_SERIAL_COM.print("Retrigger von Kanal ");
				CCONFIG_SERIAL_COM.print(i16Parameter[0]);
				CCONFIG_SERIAL_COM.println(" freigegeben");
			}
			else
			{
				CCONFIG_SERIAL_COM.print("Retrigger von Kanal ");
				CCONFIG_SERIAL_COM.print(i16Parameter[0]);
				CCONFIG_SERIAL_COM.println(" gesperrt");
			}
#else
			PrintOK();
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
		else
		{
			u32RetVal = 1L;
			PrintError(5);
		}
	}
	else
	{
		u32RetVal = 2L;
		PrintError(6);
	}

	return(u32RetVal);
}

uint32_t CInstructionDecoder::EvaluateInstruction_SxxxAF(boolean bSwitch)
{
/* Variable declarations */
	uint32_t u32RetVal;
	uint32_t u32Parameter[CINSTRUCTIONDECODER_MAX_PARAMETERS];
	uint32_t u32MaxChannel;

/* Variable initializations */
	u32RetVal = 0L;
	u32MaxChannel = MAXCHANNEL(bSwitch);

/* Implementation */
	u32Parameter[0] = atol((char*)m_szParameter[0]);
	u32Parameter[1] = atol((char*)m_szParameter[1]);

	if (u32Parameter[0] < u32MaxChannel)
	{
		if (Timer.SetAsynchronousMode(CALCCHANNEL(u32Parameter[0], bSwitch), u32Parameter[1] != 0L) == 0)
		{
#ifdef CCONFIG_DOCKLIGHT_MODE
			if (u32Parameter[1] == 0L)
			{
				CCONFIG_SERIAL_COM.print("Synchrone Betriebsart fuer ");
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal "); 
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" gesetzt");
			}
			else
			{
				CCONFIG_SERIAL_COM.print("Asynchrone Betriebsart fuer ");
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal ");
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" gesetzt");
			}
#else
			PrintOK();
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
		else
		{
			u32RetVal = 1L;
			PrintError(5);
		}
	}
	else
	{
		u32RetVal = 2L;
		PrintError(6);
	}
	
	return(u32RetVal);
}

uint32_t CInstructionDecoder::EvaluateInstruction_SxxxC(boolean bSwitch)
{
/* Variable declarations */
	uint32_t u32RetVal;
	uint32_t u32Parameter[CINSTRUCTIONDECODER_MAX_PARAMETERS];
	uint32_t u32MaxChannel;

/* Variable initializations */
	u32RetVal = 0L;
	u32MaxChannel = MAXCHANNEL(bSwitch);

	/* Implementation */
	u32Parameter[0] = atol((char*)m_szParameter[0]);
	u32Parameter[1] = atol((char*)m_szParameter[1]);

	if (u32Parameter[0] < u32MaxChannel)
	{
		if (Timer.SetCounter(CALCCHANNEL(u32Parameter[0], bSwitch), u32Parameter[1]) == 0)
		{
#ifdef CCONFIG_DOCKLIGHT_MODE
			CCONFIG_SERIAL_COM.print("Anzahl der Impulse im Impulsbetrieb von ");
			if (bSwitch == false)
			{
				CCONFIG_SERIAL_COM.print("Netzteil-");
			}
			CCONFIG_SERIAL_COM.print("Schaltkanal ");
			CCONFIG_SERIAL_COM.print(u32Parameter[0]);
			CCONFIG_SERIAL_COM.print(" auf ");
			CCONFIG_SERIAL_COM.print(u32Parameter[1]);
			CCONFIG_SERIAL_COM.println(" gesetzt");
#else
			PrintOK();
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
		else
		{
			u32RetVal = 1L;
			PrintError(5);
		}
	}
	else
	{
		u32RetVal = 2L;
		PrintError(6);
	}

	return(u32RetVal);
}

uint32_t CInstructionDecoder::EvaluateInstruction_SxxxCONFIG(boolean bSwitch)
{
	/* Annotation */
	/*
	u32Parameter[X]
	X = 0: Channel
	X = 1: Enable
	X = 2: Mode
	X = 3: Timing: On 
	X = 4: Timing: Off
	X = 5: Timing: Offset
	X = 6: Number of impulses
	*/
	/* Variable declarations */
	uint32_t u32RetVal;
	uint32_t u32Parameter[CINSTRUCTIONDECODER_MAX_PARAMETERS];
	uint32_t u32MaxChannel;

	/* Variable initializations */
	u32RetVal = 0L;
	u32MaxChannel = MAXCHANNEL(bSwitch);

	/* Implementation */
	u32Parameter[0] = atol((char*)m_szParameter[0]);
	u32Parameter[1] = atol((char*)m_szParameter[1]);
	u32Parameter[2] = atol((char*)m_szParameter[2]);
	u32Parameter[3] = atol((char*)m_szParameter[3]);
	u32Parameter[4] = atol((char*)m_szParameter[4]);
	u32Parameter[5] = atol((char*)m_szParameter[5]);
	u32Parameter[6] = atol((char*)m_szParameter[6]);
	u32Parameter[7] = atol((char*)m_szParameter[7]);
	u32Parameter[8] = atol((char*)m_szParameter[8]);
	u32Parameter[9] = atol((char*)m_szParameter[9]);
	u32Parameter[10] = atol((char*)m_szParameter[10]);
	u32Parameter[11] = atol((char*)m_szParameter[11]);
	
	if (u32Parameter[0] < u32MaxChannel)
	{
		u32RetVal += Timer.SetEnable(CALCCHANNEL(u32Parameter[0], bSwitch), (boolean)u32Parameter[1]);
		if (u32RetVal == 0)
		{
#ifdef CCONFIG_DOCKLIGHT_MODE
			if (u32Parameter[1] != 0)
			{
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal ");
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" freigegeben");
			}
			else
			{
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal ");
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" gesperrt");
			}
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}

// Mode
		if (strcmp((char*)m_szParameter[2], "OFF") == 0)
		{
			u32RetVal += Timer.SetMode(CALCCHANNEL(u32Parameter[0], bSwitch), CTIMERPIN_MODE_OFF);
#ifdef CCONFIG_DOCKLIGHT_MODE
			if (u32RetVal == 0L)
			{
				CCONFIG_SERIAL_COM.print("Betriebsmodus von ");
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal ");
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" auf 'OFF' gesetzt");
			}
			#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
		
		else if (strcmp((char*)m_szParameter[2], "ON") == 0)
		{
			u32RetVal += Timer.SetMode(u32Parameter[0],CTIMERPIN_MODE_ON);
			
#ifdef CCONFIG_DOCKLIGHT_MODE
			if (u32RetVal == 0L)
			{
				CCONFIG_SERIAL_COM.print("Betriebsmodus von ");
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal ");
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" auf 'ON' gesetzt");
			}
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
		
		else if (strcmp((char*)m_szParameter[2], "TOGGLE") == 0)
		{
			u32RetVal += Timer.SetMode(u32Parameter[0],CTIMERPIN_MODE_TOGGLE);
			
#ifdef CCONFIG_DOCKLIGHT_MODE
			if (u32RetVal == 0L)
			{
				CCONFIG_SERIAL_COM.print("Betriebsmodus von ");
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal ");
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" auf 'TOGGLE' gesetzt");
			}
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
		
		else if (strcmp((char*)m_szParameter[2], "IMPULSE") == 0)
		{
			u32RetVal += Timer.SetMode(u32Parameter[0],CTIMERPIN_MODE_IMPULSE);
			
#ifdef CCONFIG_DOCKLIGHT_MODE
			if (u32RetVal == 0L)
			{
				CCONFIG_SERIAL_COM.print("Betriebsmodus von ");
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal ");
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" auf 'IMPULSE' gesetzt");
			}
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
		else
		{
			u32RetVal += 1L;
		}
			
// Timing
		u32RetVal += Timer.SetTiming(CALCCHANNEL(u32Parameter[0], bSwitch), u32Parameter[3], u32Parameter[4], u32Parameter[5]);
		if (u32RetVal == 0L)
		{
#ifdef CCONFIG_DOCKLIGHT_MODE
			CCONFIG_SERIAL_COM.print("Zeiten von ");
			if (bSwitch == false)
			{
				CCONFIG_SERIAL_COM.print("Netzteil-");
			}
			CCONFIG_SERIAL_COM.print("Schaltkanal ");
			CCONFIG_SERIAL_COM.print(u32Parameter[0]);
			CCONFIG_SERIAL_COM.println(" wurden wie folgt eingestellt:");
			CCONFIG_SERIAL_COM.print("- Einschaltzeit:     ");
			CCONFIG_SERIAL_COM.print(u32Parameter[3]);
			CCONFIG_SERIAL_COM.println("ms");
			CCONFIG_SERIAL_COM.print("- Ausschaltzeit:     ");
			CCONFIG_SERIAL_COM.print(u32Parameter[4]);
			CCONFIG_SERIAL_COM.println("ms");
			CCONFIG_SERIAL_COM.print("- Verzoegerungszeit: ");
			CCONFIG_SERIAL_COM.print(u32Parameter[5]);
			CCONFIG_SERIAL_COM.println("ms");
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
			
// Counter 		
		u32RetVal += Timer.SetCounter(CALCCHANNEL(u32Parameter[0], bSwitch), u32Parameter[6]);
		if (u32RetVal == 0L)
		{
#ifdef CCONFIG_DOCKLIGHT_MODE
			CCONFIG_SERIAL_COM.print("Anzahl der Impulse im Impulsbetrieb von ");
			if (bSwitch == false)
			{
				CCONFIG_SERIAL_COM.print("Netzteil-");
			}
			CCONFIG_SERIAL_COM.print("Schaltkanal ");
			CCONFIG_SERIAL_COM.print(u32Parameter[0]);
			CCONFIG_SERIAL_COM.print(" auf ");
			CCONFIG_SERIAL_COM.print(u32Parameter[1]);
			CCONFIG_SERIAL_COM.println(" gesetzt");
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
			
// Asynchronous flag
		u32RetVal += Timer.SetAsynchronousMode(CALCCHANNEL(u32Parameter[0], bSwitch), u32Parameter[7] != 0L);			
		if (u32RetVal == 0L)
		{
#ifdef CCONFIG_DOCKLIGHT_MODE
			if (u32Parameter[7] == 0L)
			{
				CCONFIG_SERIAL_COM.print("Synchrone Betriebsart fuer ");
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal ");
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" gesetzt");
			}
			else
			{
				CCONFIG_SERIAL_COM.print("Asynchrone Betriebsart fuer ");
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal ");
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" gesetzt");
			}
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
		
// Immediate flag	
		u32RetVal += Timer.SetImmediateMode(CALCCHANNEL(u32Parameter[0], bSwitch), u32Parameter[8] != 0L);
		if (u32RetVal == 0L)
		{
#ifdef CCONFIG_DOCKLIGHT_MODE
			if (u32Parameter[8] == 0L)
			{
				CCONFIG_SERIAL_COM.print("Ausgabe mit Beginn der On-Phase von  ");
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal ");
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" gesetzt");
			}
			else
			{
				CCONFIG_SERIAL_COM.print("Unmittelbare Ausgabe von ");
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal ");
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" gesetzt");
			}
#endif /* CCONFIG_DOCKLIGHT_MODE */		
		}

// Negation flag
		u32RetVal += Timer.SetInvertedMode(CALCCHANNEL(u32Parameter[0], bSwitch), u32Parameter[9] != 0L);
		if (u32RetVal == 0L)
		{
#ifdef CCONFIG_DOCKLIGHT_MODE
			if (u32Parameter[9] == 0L)
			{
				CCONFIG_SERIAL_COM.print("Nicht invertierte Ausgabe von ");
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal ");
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" gesetzt");
			}
			else
			{
				CCONFIG_SERIAL_COM.print("Invertierte Ausgabe von ");
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal ");
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" gesetzt");
			}
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
		
// Trigger mode		
		u32RetVal += Timer.SetTriggerMode(CALCCHANNEL(u32Parameter[0], bSwitch), (uint8_t)u32Parameter[10], u32Parameter[11] != 0L);
		if (u32RetVal == 0L)
		{
#ifdef CCONFIG_DOCKLIGHT_MODE
			CCONFIG_SERIAL_COM.print("Trigger-Modus von  ");
			if (bSwitch == false)
			{
				CCONFIG_SERIAL_COM.print("Netzteil-");
			}
			CCONFIG_SERIAL_COM.print("Schaltkanal ");
			CCONFIG_SERIAL_COM.print(u32Parameter[0]);
			CCONFIG_SERIAL_COM.print(" auf Trigger-Kanal ");
			CCONFIG_SERIAL_COM.print(u32Parameter[10]);
			if (u32Parameter[11] == 0L)
			{
				CCONFIG_SERIAL_COM.println(" gesetzt");
			}
			else
			{
				CCONFIG_SERIAL_COM.println(" geloescht");
			}
#endif /* CCONFIG_DOCKLIGHT_MODE */		
		}
			
			
		if (u32RetVal == 0L)
		{
#ifndef CCONFIG_DOCKLIGHT_MODE
			PrintOK();
#endif /* CCONFIG_DOCKLIGHT_MODE */				
		}
		else
		{
			PrintError(5);
		}
	}
	else
	{
		/* Channel out of limits */
		u32RetVal = 2L;
		PrintError(6);
	}
		
	return(u32RetVal);
}

uint32_t CInstructionDecoder::EvaluateInstruction_SxxxE(boolean bSwitch)
{
/* Variable declarations */
	uint32_t u32RetVal;
	uint32_t u32Parameter[CINSTRUCTIONDECODER_MAX_PARAMETERS];
	uint32_t u32MaxChannel;

/* Variable initializations */
	u32RetVal = 0L;
	u32MaxChannel = MAXCHANNEL(bSwitch);

	/* Implementation */
	u32Parameter[0] = atol((char*)m_szParameter[0]);
	u32Parameter[1] = atol((char*)m_szParameter[1]);

	if (u32Parameter[0] < u32MaxChannel)
	{
		if (Timer.SetEnable(CALCCHANNEL(u32Parameter[0], bSwitch), (boolean)u32Parameter[1]) == 0)
		{
#ifdef CCONFIG_DOCKLIGHT_MODE
			if (u32Parameter[1] != 0)
			{
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal ");
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" freigegeben");
			}
			else
			{
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal ");
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" gesperrt");
			}
#else
			PrintOK();
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
		else
		{
			u32RetVal = 1L;
			PrintError(5);
		}
	}
	else
	{
		u32RetVal = 2L;
		PrintError(6);
	}
	
	return(u32RetVal);
}

uint32_t CInstructionDecoder::EvaluateInstruction_SxxxIF(boolean bSwitch)
{
/* Variable declarations */
	uint32_t u32RetVal;
	uint32_t u32Parameter[CINSTRUCTIONDECODER_MAX_PARAMETERS];
	uint32_t u32MaxChannel;

/* Variable initializations */
	u32RetVal = 0L;
	u32MaxChannel = MAXCHANNEL(bSwitch);

/* Implementation */
	u32Parameter[0] = atol((char*)m_szParameter[0]);
	u32Parameter[1] = atol((char*)m_szParameter[1]);

	if (u32Parameter[0] < u32MaxChannel)
	{
		if (Timer.SetImmediateMode(CALCCHANNEL(u32Parameter[0], bSwitch), u32Parameter[1] != 0L) == 0)
		{
#ifdef CCONFIG_DOCKLIGHT_MODE
			if (u32Parameter[1] == 0L)
			{
				CCONFIG_SERIAL_COM.print("Ausgabe mit Beginn der On-Phase von  ");
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal "); 
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" gesetzt");
			}
			else
			{
				CCONFIG_SERIAL_COM.print("Unmittelbare Ausgabe von ");
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal ");
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" gesetzt");
			}
#else
			PrintOK();
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
		else
		{
			u32RetVal = 1L;
			PrintError(5);
		}
	}
	else
	{
		u32RetVal = 2L;
		PrintError(6);
	}
	
	return(u32RetVal);
}

uint32_t CInstructionDecoder::EvaluateInstruction_SxxxM(boolean bSwitch)
{
/* Variable declarations */
	uint32_t u32RetVal;
	uint32_t u32Parameter[CINSTRUCTIONDECODER_MAX_PARAMETERS];
	uint32_t u32MaxChannel;

/* Variable initializations */
	u32RetVal = 0L;
	u32MaxChannel = MAXCHANNEL(bSwitch);

/* Implementation */
	u32Parameter[0] = atol((char*)m_szParameter[0]);
	
	if (u32Parameter[0] < u32MaxChannel)
	{
		if (strcmp((char*)m_szParameter[1], "OFF") == 0)
		{
			u32RetVal = Timer.SetMode(CALCCHANNEL(u32Parameter[0], bSwitch), CTIMERPIN_MODE_OFF);
			
#ifdef CCONFIG_DOCKLIGHT_MODE
			if (u32RetVal == 0L)
			{
				CCONFIG_SERIAL_COM.print("Betriebsmodus von ");
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal ");
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" auf 'OFF' gesetzt");
			}
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
		
		else if (strcmp((char*)m_szParameter[1], "ON") == 0)
		{
			u32RetVal = Timer.SetMode(CALCCHANNEL(u32Parameter[0], bSwitch),CTIMERPIN_MODE_ON);
			
#ifdef CCONFIG_DOCKLIGHT_MODE
			if (u32RetVal == 0L)
			{
				CCONFIG_SERIAL_COM.print("Betriebsmodus von ");
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal ");
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" auf 'ON' gesetzt");
			}
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
		
		else if (strcmp((char*)m_szParameter[1], "TOGGLE") == 0)
		{
			u32RetVal = Timer.SetMode(CALCCHANNEL(u32Parameter[0], bSwitch),CTIMERPIN_MODE_TOGGLE);
			
#ifdef CCONFIG_DOCKLIGHT_MODE
			if (u32RetVal == 0L)
			{
				CCONFIG_SERIAL_COM.print("Betriebsmodus von ");
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal ");
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" auf 'TOGGLE' gesetzt");
			}
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
		
		else if (strcmp((char*)m_szParameter[1], "IMPULSE") == 0)
		{
			u32RetVal = Timer.SetMode(CALCCHANNEL(u32Parameter[0], bSwitch),CTIMERPIN_MODE_IMPULSE);
			
#ifdef CCONFIG_DOCKLIGHT_MODE
			if (u32RetVal == 0L)
			{
				CCONFIG_SERIAL_COM.print("Betriebsmodus von ");
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal ");
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" auf 'IMPULSE' gesetzt");
			}
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
		else
		{
			u32RetVal = 6L;
		}
	}
	else
	{
		u32RetVal = 6L;
	}
/* Examination of u32RetVal */
	if (u32RetVal != 0L)
	{
		switch (u32RetVal)
		{
		case 6:
			PrintError(6);
			break;
			
		default:
			PrintError(5);
			break;
		}
	}
#ifndef CCONFIG_DOCKLIGHT_MODE
	else
	{
		PrintOK();
	}
#endif /* CCONFIG_DOCKLIGHT_MODE */

	return(u32RetVal);
}


uint32_t CInstructionDecoder::EvaluateInstruction_SxxxNF(boolean bSwitch)
{
/* Variable declarations */
	uint32_t u32RetVal;
	uint32_t u32Parameter[CINSTRUCTIONDECODER_MAX_PARAMETERS];
	uint32_t u32MaxChannel;

/* Variable initializations */
	u32RetVal = 0L;
	u32MaxChannel = MAXCHANNEL(bSwitch);

/* Implementation */
	u32Parameter[0] = atol((char*)m_szParameter[0]);
	u32Parameter[1] = atol((char*)m_szParameter[1]);

	if (u32Parameter[0] < u32MaxChannel)
	{
		if (Timer.SetInvertedMode(CALCCHANNEL(u32Parameter[0], bSwitch), u32Parameter[1] != 0L) == 0)
		{
#ifdef CCONFIG_DOCKLIGHT_MODE
			if (u32Parameter[1] == 0L)
			{
				CCONFIG_SERIAL_COM.print("Nicht invertierte Ausgabe von ");
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal "); 
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" gesetzt");
			}
			else
			{
				CCONFIG_SERIAL_COM.print("Invertierte Ausgabe von ");
				if (bSwitch == false)
				{
					CCONFIG_SERIAL_COM.print("Netzteil-");
				}
				CCONFIG_SERIAL_COM.print("Schaltkanal ");
				CCONFIG_SERIAL_COM.print(u32Parameter[0]);
				CCONFIG_SERIAL_COM.println(" gesetzt");
			}
#else
			PrintOK();
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
		else
		{
			u32RetVal = 1L;
			PrintError(5);
		}
	}
	else
	{
		u32RetVal = 2L;
		PrintError(6);
	}
	
	return(u32RetVal);
}

uint32_t CInstructionDecoder::EvaluateInstruction_SxxxT(boolean bSwitch)
{
/* Variable declarations */
	uint32_t u32RetVal;
	uint32_t u32Parameter[CINSTRUCTIONDECODER_MAX_PARAMETERS];
	uint32_t u32MaxChannel;

/* Variable initializations */
	u32RetVal = 0L;
	u32MaxChannel = MAXCHANNEL(bSwitch);

/* Implementation */
	u32Parameter[0] = (uint32_t)atol((char*)m_szParameter[0]);
	u32Parameter[1] = (uint32_t)atol((char*)m_szParameter[1]);
	u32Parameter[2] = (uint32_t)atol((char*)m_szParameter[2]);
	u32Parameter[3] = (uint32_t)atol((char*)m_szParameter[3]);

	if (u32Parameter[0] < u32MaxChannel)
	{
		u32RetVal = Timer.SetTiming(CALCCHANNEL(u32Parameter[0], bSwitch), u32Parameter[1], u32Parameter[2], u32Parameter[3]);
		if (u32RetVal == 0L)
		{
#ifdef CCONFIG_DOCKLIGHT_MODE
			CCONFIG_SERIAL_COM.print("Zeiten von ");
			if (bSwitch == false)
			{
				CCONFIG_SERIAL_COM.print("Netzteil-");
			}
			CCONFIG_SERIAL_COM.print("Schaltkanal ");
			CCONFIG_SERIAL_COM.print(u32Parameter[0]);
			CCONFIG_SERIAL_COM.println(" wurden wie folgt eingestellt:");
			CCONFIG_SERIAL_COM.print("- Einschaltzeit:     ");
			CCONFIG_SERIAL_COM.print(u32Parameter[1]);
			CCONFIG_SERIAL_COM.println("ms");
			CCONFIG_SERIAL_COM.print("- Ausschaltzeit:     ");
			CCONFIG_SERIAL_COM.print(u32Parameter[2]);
			CCONFIG_SERIAL_COM.println("ms");
			CCONFIG_SERIAL_COM.print("- Verzoegerungszeit: ");
			CCONFIG_SERIAL_COM.print(u32Parameter[3]);
			CCONFIG_SERIAL_COM.println("ms");
#else
			PrintOK();
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
		else
		{
			PrintError(5);
		}
	}
	else
	{
		u32RetVal = 2L;
		PrintError(6);
	}

	return(u32RetVal);
}

uint32_t CInstructionDecoder::EvaluateInstruction_SxxxTF(boolean bSwitch)
{
/* Variable declarations */
	uint32_t u32RetVal;
	uint32_t u32Parameter[CINSTRUCTIONDECODER_MAX_PARAMETERS];
	uint32_t u32MaxChannel;

/* Variable initializations */
	u32RetVal = 0L;
	u32MaxChannel = MAXCHANNEL(bSwitch);

/* Implementation */
	u32Parameter[0] = atol((char*)m_szParameter[0]);
	u32Parameter[1] = atol((char*)m_szParameter[1]);
	u32Parameter[2] = atol((char*)m_szParameter[2]);

	if ((u32Parameter[0] < u32MaxChannel) && (u32Parameter[1] < CCONFIG_TRIGGERMODULE_IOS))
	{
		if (Timer.SetTriggerMode(CALCCHANNEL(u32Parameter[0], bSwitch), (uint8_t)u32Parameter[1], u32Parameter[2] != 0L) == 0)
		{
#ifdef CCONFIG_DOCKLIGHT_MODE
			CCONFIG_SERIAL_COM.print("Trigger-Modus von  ");
			if (bSwitch == false)
			{
				CCONFIG_SERIAL_COM.print("Netzteil-");
			}
			CCONFIG_SERIAL_COM.print("Schaltkanal "); 
			CCONFIG_SERIAL_COM.print(u32Parameter[0]);
			CCONFIG_SERIAL_COM.print(" auf Trigger-Kanal ");
			CCONFIG_SERIAL_COM.print(u32Parameter[1]);
			if (u32Parameter[1] == 0L)
			{
				CCONFIG_SERIAL_COM.println(" gesetzt");
			}
			else
			{
				CCONFIG_SERIAL_COM.println(" geloescht");
			}
#else
			PrintOK();
#endif /* CCONFIG_DOCKLIGHT_MODE */
		}
		else
		{
			u32RetVal = 1L;
			PrintError(5);
		}
	}
	else
	{
		u32RetVal = 2L;
		PrintError(6);
	}
	
	return(u32RetVal);
}

long CInstructionDecoder::GetLongParameter(int i16Parameter)	
{
	long i32RetVal;

	sscanf((char*)m_szParameter[i16Parameter],"%ld",&i32RetVal);
	
	return i32RetVal;
}	
	
float CInstructionDecoder::GetFloatParameter(int i16Parameter)
{
	float f32RetVal;

	sscanf((char*)m_szParameter[i16Parameter],"%f",&f32RetVal);
	
	return f32RetVal;
}		

void CInstructionDecoder::Info(void)
{
/* Variable declarations */
	CTIMERPIN_MODE_T Mode;
	CTRIGGER_MODE_T TriggerMode;
	CTIMERPIN_TRIGGER_T TriggerPin;
	char szString[150];
//	char szBool[10][5];
	char szBool[5];
	char szMode[10];
	char szData[10];
	uint8_t u8Channel;
	uint8_t u8SwitchChannel;
	boolean bData;
	boolean bAppendData;
	float f32Data;
	uint32_t u32Data;
	uint32_t u32TimeOffMS;
	uint32_t u32TimeOnMS;
	uint32_t u32OffsetMS;

/* Variable initializations */

/* Implementation */
#ifndef CCONFIG_DOCKLIGHT_MODE
	PrintOK();
#else
#ifdef CCONFIG_RELAYMODULE
	CCONFIG_SERIAL_COM.println();
	CCONFIG_SERIAL_COM.println("Informationen zu den wichtigsten Parametern");
	CCONFIG_SERIAL_COM.println("===========================================");
	CCONFIG_SERIAL_COM.println();

	CCONFIG_SERIAL_COM.println("Konfiguration:");
	CCONFIG_SERIAL_COM.println("--------------");
	Config.Get(CCONFIG_WHAT_ENABLE_BY_RETAIN, &bData);
	BoolToString(MODE_YES_NO, bData, szBool);
	sprintf(szString, "- Schaltkanalfreigaben im Eeprom ablegen: %s", szBool);
	CCONFIG_SERIAL_COM.println(szString);
	CCONFIG_SERIAL_COM.println();
	
	if (CCONFIG_RELAYMODULE_IOS > 0)
	{
		CCONFIG_SERIAL_COM.println("Schaltkanaele (Schaltmodul):");
		CCONFIG_SERIAL_COM.println("---------------------------");
		CCONFIG_SERIAL_COM.println("Kanal   Enabled   AF  IF  NF  TRG    Mode       Einschaltzeit  Ausschaltzeit  Zeitverzoegerung   TriggerIO   Status");
		CCONFIG_SERIAL_COM.println("-------------------------------------------------------------------------------------------------------------------");
	
		for(u8Channel = 0; u8Channel < CCONFIG_RELAYMODULE_IOS; u8Channel++)
		{
			sprintf(szString, "%3d", u8Channel);

			Timer.GetEnable(u8Channel, bData);
			BoolToString(MODE_X, bData, szBool);
			sprintf(szString, "%s%9s", szString, szBool);
		
			Timer.GetAsynchronousMode(u8Channel, bData);
			BoolToString(MODE_X, bData, szBool);
			sprintf(szString, "%s%7s", szString,szBool);
		
			Timer.GetImmediateMode(u8Channel, bData);
			BoolToString(MODE_X, bData, szBool);
			sprintf(szString, "%s%4s", szString, szBool);
		
			Timer.GetInvertedMode(u8Channel, bData);
			BoolToString(MODE_X, bData, szBool);
			sprintf(szString, "%s%4s", szString, szBool);
		
			Timer.GetTriggerMode(u8Channel, bData);
			BoolToString(MODE_X, bData, szBool);
			sprintf(szString, "%s%5s", szString, szBool);

			Timer.GetMode(u8Channel, Mode);
			switch (Mode)
			{
			case CTIMERPIN_MODE_IMPULSE:
				strcpy(szMode, "Impuls");
				break;
			case CTIMERPIN_MODE_OFF:	
				strcpy(szMode, " Aus  ");
				break;
			case CTIMERPIN_MODE_ON:
				strcpy(szMode, " Ein  ");
				break;

			case CTIMERPIN_MODE_TOGGLE:
				strcpy(szMode, "Toggle");
				break;

			default:
				strcpy(szMode, "???");
				break;
			}
			sprintf(szString, "%s%10s", szString, szMode);
		
		
			Timer.GetTiming(u8Channel, u32TimeOnMS, u32TimeOffMS, u32OffsetMS);
			sprintf(szString, "%s%15d[ms]", szString, u32TimeOnMS);
			sprintf(szString, "%s%11d[ms]", szString, u32TimeOffMS);
			sprintf(szString, "%s%14d[ms]", szString, u32OffsetMS);
			
			Timer.GetTriggerMode(u8Channel, TriggerPin, bData);
			TriggerIOToString(TriggerPin, szData);
			sprintf(szString,"%s%9s", szString, szData);
		
			Timer.GetState(u8Channel, bData);
			BoolToString(MODE_ON_OFF, bData, szBool);
			sprintf(szString, "%s%11s", szString,szBool);

			CCONFIG_SERIAL_COM.println(szString);
			CCONFIG_SERIAL_COM.println();
		}
	}
#endif /* CCONFIG_RELAYMODULE */
	
#ifdef CCONFIG_POWERSUPPLY
	if (CCONFIG_POWERSUPPLY_IOS > 0)
	{
		CCONFIG_SERIAL_COM.println();
	
		CCONFIG_SERIAL_COM.println("Schaltkanaele (Netzteil):");
		CCONFIG_SERIAL_COM.println("---------------------------");
		CCONFIG_SERIAL_COM.println("Kanal   Enabled   AF  IF  NF  TRG    Mode       Einschaltzeit  Ausschaltzeit  Zeitverzoegerung   TriggerIO   Status");
		CCONFIG_SERIAL_COM.println("-------------------------------------------------------------------------------------------------------------------");
	
		for(u8Channel = CCONFIG_RELAYMODULE_IOS; u8Channel < CCONFIG_IOS; u8Channel++)
		{
			ChannelToString(u8Channel, szString);

			Timer.GetEnable(u8Channel, bData);
			BoolToString(MODE_X, bData, szBool);
			sprintf(szString, "%s%7s", szString, szBool);
		
			Timer.GetAsynchronousMode(u8Channel, bData);
			BoolToString(MODE_X, bData, szBool);
			sprintf(szString, "%s%7s", szString,szBool);
		
			Timer.GetImmediateMode(u8Channel, bData);
			BoolToString(MODE_X, bData, szBool);
			sprintf(szString, "%s%4s", szString, szBool);
		
			Timer.GetInvertedMode(u8Channel, bData);
			BoolToString(MODE_X, bData, szBool);
			sprintf(szString, "%s%4s", szString, szBool);
		
			Timer.GetTriggerMode(u8Channel, bData);
			BoolToString(MODE_X, bData, szBool);
			sprintf(szString, "%s%5s", szString, szBool);

			Timer.GetMode(u8Channel, Mode);
			switch (Mode)
			{
				case CTIMERPIN_MODE_IMPULSE:
				strcpy(szMode, "Impuls");
				break;
				case CTIMERPIN_MODE_OFF:
				strcpy(szMode, " Aus  ");
				break;
				case CTIMERPIN_MODE_ON:
				strcpy(szMode, " Ein  ");
				break;

				case CTIMERPIN_MODE_TOGGLE:
				strcpy(szMode, "Toggle");
				break;

				default:
				strcpy(szMode, "???");
				break;
			}
			sprintf(szString, "%s%10s", szString, szMode);
		
		
			Timer.GetTiming(u8Channel, u32TimeOnMS, u32TimeOffMS, u32OffsetMS);
			sprintf(szString, "%s%15d[ms]", szString, u32TimeOnMS);
			sprintf(szString, "%s%11d[ms]", szString, u32TimeOffMS);
			sprintf(szString, "%s%14d[ms]", szString, u32OffsetMS);
		
			Timer.GetTriggerMode(u8Channel, TriggerPin, bData);
			TriggerIOToString(TriggerPin, szData);
			sprintf(szString,"%s%9s", szString, szData);
			
			Timer.GetState(u8Channel, bData);
			BoolToString(MODE_ON_OFF, bData, szBool);
			sprintf(szString, "%s%11s", szString,szBool);

			CCONFIG_SERIAL_COM.println(szString);
			CCONFIG_SERIAL_COM.println();
			CCONFIG_SERIAL_COM.println();
		}
	}
#endif /* CCONFIG_POWERSUPPLY */
	
#ifdef CCONFIG_TRIGGERMODULE	
	if (CCONFIG_TRIGGERMODULE_IOS > 0)
	{
		CCONFIG_SERIAL_COM.println("Trigger:");
		CCONFIG_SERIAL_COM.println("--------");

		CCONFIG_SERIAL_COM.println("Kanal   Enabled   RF    Mode    Level       triggert Schaltkanal");
		CCONFIG_SERIAL_COM.println("----------------------------------------------------------------");
		
		for (u8Channel = 0; u8Channel < CCONFIG_TRIGGERMODULE_IOS; u8Channel++)
		{
			sprintf(szString, "%3d", u8Channel);

			bData = Trigger.IsEnabled(u8Channel);
			BoolToString(MODE_X, bData, szBool);
			sprintf(szString, "%s%9s", szString, szBool);
			
			bData = Trigger.IsRetrigger(u8Channel);
			BoolToString(MODE_X, bData, szBool);
			sprintf(szString, "%s%7s", szString, szBool);
			
			Trigger.GetMode(u8Channel, TriggerMode);
			TriggerModeToString(TriggerMode, szMode);
			sprintf(szString, "%s%10s", szString, szMode);
			
			Trigger.GetTriggerLevel(u8Channel, f32Data);
			u32Data = (uint32_t)(100*f32Data);
			u32Data /= 100;
			sprintf(szString, "%s%4d", szString, u32Data);
			u32Data = (uint32_t)(100*f32Data);
			u32Data %= 100;
			sprintf(szString, "%s.%02dV", szString, u32Data);

			bAppendData = false;
			for(u8SwitchChannel = 0; u8SwitchChannel < CCONFIG_IOS; u8SwitchChannel++)
			{
				Timer.GetTriggerMode(u8SwitchChannel, TriggerPin, bData);
				if ((TriggerPin != CTIMERPIN_TRIGGER_OFF)&&(TriggerPin == u8Channel))
				{
					if (bAppendData == false)
					{
						if (u8SwitchChannel < CCONFIG_RELAYMODULE_IOS)
						{
							sprintf(szString, "%s       CH%d", szString, u8SwitchChannel);
						} 
						else
						{
							switch (u8SwitchChannel)
							{
							case CCONFIG_RELAYMODULE_IOS:
								sprintf(szString, "%s       3.3V", szString);
								break;
								
							case CCONFIG_RELAYMODULE_IOS + 1:
								sprintf(szString, "%s       5V", szString);
								break;
							
							case CCONFIG_RELAYMODULE_IOS + 2:
								sprintf(szString, "%s       12V", szString);
								break;
							
							case CCONFIG_RELAYMODULE_IOS + 3:
								sprintf(szString, "%s       -12V", szString);
								break;
								
							default:
								sprintf(szString, "%s       ???", szString);
								break;
							}
						}
						bAppendData = true;
					} 
					else
					{
						if (u8SwitchChannel < CCONFIG_RELAYMODULE_IOS)
						{
							sprintf(szString, "%s, CH%", szString, u8SwitchChannel);
						}
						else
						{
							switch (u8SwitchChannel)
							{
							case CCONFIG_RELAYMODULE_IOS:
								sprintf(szString, "%s, 3.3V", szString);
								break;
								
							case CCONFIG_RELAYMODULE_IOS + 1:
								sprintf(szString, "%s, 5V", szString);
								break;
								
							case CCONFIG_RELAYMODULE_IOS + 2:
								sprintf(szString, "%s, 12V", szString);
								break;
								
							case CCONFIG_RELAYMODULE_IOS + 3:
								sprintf(szString, "%s, -12V", szString);
								break;
							}
						}
					}
				}
			}


			CCONFIG_SERIAL_COM.println(szString);
			CCONFIG_SERIAL_COM.println();
		}
	}
#endif /* CCONFIG_TRIGGERMODULE */
	CCONFIG_SERIAL_COM.println("Legende:");
	CCONFIG_SERIAL_COM.println("--------");
	CCONFIG_SERIAL_COM.println("AF:  Asynchron Flag (Ja: Signal laeuft nicht synchron mit den anderen Signalen  Nein: Signal laeuft synchron mit den anderen Signalen)");
	CCONFIG_SERIAL_COM.println("IF:  Immediate Flag (Ja: Signal wird unmittelbar nach der Freigabe gesetzt      Nein: Signal startet mit der ersten ansteigenden Flanke)");
	CCONFIG_SERIAL_COM.println("NF:  Negations Flag (Ja: Signal wird invertiert ausgegeben                      Nein: Signal wird nicht invertiert ausgegeben)");
	CCONFIG_SERIAL_COM.println("TRG: Trigger Flag   (Ja: Signal startet ueber externen Trigger                  Nein: Signal startet mit Freigabe)");
	CCONFIG_SERIAL_COM.println("RF:  Retrigger Flag (Ja: Mehrmaliges Triggern moeglich                          Nein: Nur einmaliges Triggern moeglich)");
#endif /* CCONFIG_DOCKLIGHT_MODE */
}

void CInstructionDecoder::Init(void)
{
/* Initialize variable */
	m_bSTXReceived = 0;
	m_bFrameReceived = 0;
	m_i16RecBufferPointer = 0;

/* Open serial communications and wait for port to open: */
  
    CCONFIG_SERIAL_COM.begin(CCONFIG_SERIAL_COM_BAUDRATE, CCONFIG_SERIAL_COM_PARAM);

	Debug.Log(DEBUG_FLAG_ID_INFORMATIONS, "Kommunikations-Schnittstelle mit ", CCONFIG_SERIAL_COM_BAUDRATE, false);
	Debug.Log(DEBUG_FLAG_ID_INFORMATIONS, "baud initialisiert", true);
}	


#ifndef CCONFIG_DOCKLIGHT_MODE
void CInstructionDecoder::PrintData(char* szData)
{
/* Variable declarations */
	char szString[2];

/* Variable initializations */

/* Implementation */
	sprintf(szString, "%c", 0x02);
	CCONFIG_SERIAL_COM.print(szString);
	CCONFIG_SERIAL_COM.print(szData);
	sprintf(szString, "%c", 0x03);
	CCONFIG_SERIAL_COM.print(szString);
}

void CInstructionDecoder::PrintData(uint8_t u8Data)
{
	/* Variable declarations */
	char szString[8];
	
	/* Variable initializations */
	
	/* Implementation */
	sprintf(szString, "%c%d%c", 0x02, u8Data, 0x03);
	CCONFIG_SERIAL_COM.print(szString);
}

void CInstructionDecoder::PrintData(bool bData)
{
	/* Variable declarations */
	char szString[8];
	uint8_t u8Data;

	/* Variable initializations */
	u8Data = bData? 1 : 0;

	/* Implementation */
	sprintf(szString, "%c%d%c", 0x02, u8Data, 0x03);
	CCONFIG_SERIAL_COM.print(szString);
}

void CInstructionDecoder::PrintData(int8_t i8Data)
{
	/* Variable declarations */
	char szString[8];

	/* Variable initializations */

	/* Implementation */
	sprintf(szString, "%c%d%c", 0x02, i8Data, 0x03);
	CCONFIG_SERIAL_COM.print(szString);
}

void CInstructionDecoder::PrintData(uint32_t u32Data)
{
/* Variable declarations */
	char szString[16];
	
/* Variable initializations */
	
/* Implementation */
	sprintf(szString, "%c%ld%c", 0x02, u32Data, 0x03);
	CCONFIG_SERIAL_COM.print(szString);
}

void CInstructionDecoder::PrintData(int32_t i32Data)
{
/* Variable declarations */
	char szString[16];

/* Variable initializations */

/* Implementation */
	sprintf(szString, "%c%ld%c", 0x02, i32Data, 0x03);
	CCONFIG_SERIAL_COM.print(szString);
}

void CInstructionDecoder::PrintData(float fData)
{
/* Variable declarations */
	uint8_t u8Delimiter;
	char szString[2];

/* Variable initializations */
	u8Delimiter = 0x02;

/* Implementation */
	sprintf(szString, "%c", 0x02);
	CCONFIG_SERIAL_COM.print(szString);
	CCONFIG_SERIAL_COM.print(fData);
	sprintf(szString, "%c", 0x03);
	CCONFIG_SERIAL_COM.print(szString);
}

void CInstructionDecoder::PrintData(uint32_t u32Data[], uint8_t u8Size)
{
/* Variable declarations */
	uint8_t u8Delimiter;
	char szString[2];
	uint8_t u8Size_Copy;

/* Variable initializations */
	u8Size_Copy = u8Size;
	u8Delimiter = 0x02;

/* Implementation */
	sprintf(szString, "%c", 0x02);
	CCONFIG_SERIAL_COM.print(szString);
	
	while (u8Size_Copy)
	{
		CCONFIG_SERIAL_COM.print(u32Data[u8Size - u8Size_Copy]);
		
		u8Size_Copy--;
		
		if ((--u8Size_Copy) > 0)
		{
			sprintf(szString, "%c", CINSTRUCTIONDECODER_SEPARATOR);
			CCONFIG_SERIAL_COM.print(szString);
		}

	}

	sprintf(szString, "%c", 0x03);
	CCONFIG_SERIAL_COM.print(szString);
}

#endif /* CCONFIG_DOCKLIGHT_MODE */

void CInstructionDecoder::PrintError(uint8_t u8Error)
{
/* Variable declarations */
#ifndef CCONFIG_DOCKLIGHT_MODE
	char szString[8];
#endif /* CCONFIG_DOCKLIGHT_MODE */

/* Variable initializations */

/* Implementation */
#ifdef CCONFIG_DOCKLIGHT_MODE
	switch (u8Error)
	{
	case 4:
		CCONFIG_SERIAL_COM.println(E004);
		break;

	case 5:
		CCONFIG_SERIAL_COM.println(E005);
		break;

	case 6:
		CCONFIG_SERIAL_COM.println(E006);
		break;
	default:	
		CCONFIG_SERIAL_COM.println(E099);
		break;
	}
	
#else
	sprintf(szString, "%cE%03d%c", 0x02, u8Error, 0x03);
	CCONFIG_SERIAL_COM.print(szString);
#endif /* CCONFIG_DOCKLIGHT_MODE */
}

#ifndef CCONFIG_DOCKLIGHT_MODE
void CInstructionDecoder::PrintNOK(void)
{
/* Variable declarations */
	char szString[8];

/* Variable initializations */

/* Implementation */
	sprintf(szString, "%cNOK%c", 0x02, 0x03);
	CCONFIG_SERIAL_COM.print(szString);
}

void CInstructionDecoder::PrintOK(void)
{
	/* Variable declarations */
	char szString[8];

	/* Variable initializations */

	/* Implementation */
	sprintf(szString, "%cOK%c", 0x02, 0x03);
	CCONFIG_SERIAL_COM.print(szString);
}
#endif /* CCONFIG_DOCKLIGHT_MODE */

boolean CInstructionDecoder::Receive(void)	
{
	int i16BytesinBuffer;
	unsigned char u8ByteRead;
	boolean bRetVal;
	
	bRetVal = false;	
	
	i16BytesinBuffer = CCONFIG_SERIAL_COM.available();
	
	while (i16BytesinBuffer > 0)
	{
		i16BytesinBuffer--;	
		u8ByteRead = CCONFIG_SERIAL_COM.read();

		if (u8ByteRead == STX)
		{
			m_bSTXReceived = true;
			m_i16RecBufferPointer = 0;
		}
		else
		{	
			if (m_bSTXReceived == true)
			{
				if(u8ByteRead == ETX)
				{
					m_bFrameReceived = true;

					Debug.Log(DEBUG_FLAG_ID_INFORMATIONS, "Frame '", false);
					Debug.Log(DEBUG_FLAG_ID_INFORMATIONS, (char*)m_u8RecBuffer, false);
					Debug.Log(DEBUG_FLAG_ID_INFORMATIONS, "' received", false);
					delay(50);

					bRetVal = true;
				}
				else
				{
					m_u8RecBuffer[m_i16RecBufferPointer++] = u8ByteRead;
					m_u8RecBuffer[m_i16RecBufferPointer] = 0;
					
					if(m_i16RecBufferPointer >= CINSTRUCTIONDECODER_RECBUFFERSIZE)
					{
						m_bSTXReceived = false;
					}	
				}				 
			}
		}					
	}
	
	return bRetVal;		
}

void CInstructionDecoder::Task(void)
{
	if (Receive())
	{
		EvaluateFrame();
		EvaluateInstruction();
		ClearInstruction();
	}		
		
}

void CInstructionDecoder::ClearScreen(void)
{
#ifdef CCONFIG_DOCKLIGHT_MODE
/* Variable declarations */
	char szString[8];

/* Variable initializations */

/* Implementation */
	sprintf(szString, "%c%c%c", 0x02, 0x1A, 0x03);
	CCONFIG_SERIAL_COM.print(szString);
#endif /* CCONFIG_DOCKLIGHT_MODE */
}

void CInstructionDecoder::TriggerIOToString(uint8_t u8TriggerIO, char* szString)
{
#ifdef CCONFIG_TRIGGERMODULE	
/* Variable declarations */

/* Variable initializations */

/* Implementation */
	if (u8TriggerIO < CCONFIG_TRIGGERMODULE_IOS)
	{
		sprintf(szString,"%d", u8TriggerIO);
	}
	else
	{
		strcpy(szString, "---");
	}
#else
	*szString = NULL;
#endif /* CCONFIG_TRIGGERMODULE */	
}

void CInstructionDecoder::TriggerModeToString(uint8_t u8Mode, char* szString)
{
/* Variable declarations */

/* Variable initializations */

/* Implementation */
	switch (u8Mode)
	{
	case CTRIGGER_MODE_LOW:
		strcpy(szString, "   LOW  ");
		break;

	case CTRIGGER_MODE_HIGH:
		strcpy(szString, "  HIGH  ");
		break;

	case CTRIGGER_MODE_FALLING:
		strcpy(szString, " FALLING");
		break;

	case CTRIGGER_MODE_RISING:
		strcpy(szString, " RISING ");
		break;

	case CTRIGGER_MODE_LEVEL_DOWN:
		strcpy(szString, "LEVEL_DN");
		break;

	case CTRIGGER_MODE_LEVEL_UP:
		strcpy(szString, "LEVEL_UP");
		break;

	case CTRIGGER_MODE_IRQ_DOWN:
		strcpy(szString, " IRQ_DN ");
		break;

	case CTRIGGER_MODE_IRQ_UP:
		strcpy(szString, " IRQ_UP ");
		break;

	default:
		strcpy(szString, "   ???  ");
		break;
		
	}
}
