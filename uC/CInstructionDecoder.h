//-------------------------------------------------------------------
#ifndef __CINSTRUCTIONDECODER_H__
#define __CINSTRUCTIONDECODER_H__
//-------------------------------------------------------------------

#include <arduino.h>
#include "CConfig.h"

//-------------------------------------------------------------------
#define CINSTRUCTIONDECODER_RECBUFFERSIZE		128
#define CINSTRUCTIONDECODER_MAX_PARAMETERS		12
#define CINSTRUCTIONDECODER_SEPARATOR				0x20		/* SPACE */

#define STX																	0x02 
#define ETX																	0x03 

//-------------------------------------------------------------------
//-------------------------------------------------------------------


class CInstructionDecoder
{
	public:
		CInstructionDecoder(void);
		~CInstructionDecoder();
		void Init(void);
		void Task(void);
		void ClearScreen(void);
				
	private:
		void BoolToString(boolean bData, char* szString);
		void BoolToString(uint8_t u8Mode, boolean bData, char* szString);
		void ChannelToString(uint8_t u8Channel, char* szString);		/* Nur für Netzteil */
		boolean CheckServoID(int i16ServoID);
		void ClearInstruction(void);
		void EvaluateFrame(void);	
		void EvaluateInstruction(void);
		uint32_t EvaluateInstruction_GCFG(void);

		uint32_t EvaluateInstruction_GxxxAF(boolean bSwitch);
		uint32_t EvaluateInstruction_GxxxC(boolean bSwitch);
		uint32_t EvaluateInstruction_GxxxCA(boolean bSwitch);
		uint32_t EvaluateInstruction_GxxxCONFIG(boolean bSwitch);
		uint32_t EvaluateInstruction_GxxxE(boolean bSwitch);
		uint32_t EvaluateInstruction_GxxxIF(boolean bSwitch);
		uint32_t EvaluateInstruction_GxxxM(boolean bSwitch);
		uint32_t EvaluateInstruction_GxxxNF(boolean bSwitch);
		uint32_t EvaluateInstruction_GxxxT(boolean bSwitch);
		uint32_t EvaluateInstruction_GxxxTF(boolean bSwitch);

		uint32_t EvaluateInstruction_GTRGE(void);
		uint32_t EvaluateInstruction_GTRGL(void);
		uint32_t EvaluateInstruction_GTRGM(void);
		uint32_t EvaluateInstruction_GTRGR(void);
		uint32_t EvaluateInstruction_SCFG(void);
		
		uint32_t EvaluateInstruction_STRGE(void);
		uint32_t EvaluateInstruction_STRGL(void);
		uint32_t EvaluateInstruction_STRGM(void);
		uint32_t EvaluateInstruction_STRGR(void);

		uint32_t EvaluateInstruction_SxxxAF(boolean bSwitch);
		uint32_t EvaluateInstruction_SxxxC(boolean bSwitch);
		uint32_t EvaluateInstruction_SxxxCONFIG(boolean bSwitch);
		uint32_t EvaluateInstruction_SxxxE(boolean bSwitch);
		uint32_t EvaluateInstruction_SxxxIF(boolean bSwitch);
		uint32_t EvaluateInstruction_SxxxM(boolean bSwitch);
		uint32_t EvaluateInstruction_SxxxNF(boolean bSwitch);
		uint32_t EvaluateInstruction_SxxxT(boolean bSwitch);
		uint32_t EvaluateInstruction_SxxxTF(boolean bSwitch);

		long GetLongParameter(int i16Parameter);	
		float GetFloatParameter(int i16Parameter);
		void Info(void);
		boolean Receive(void);
		void TriggerIOToString(uint8_t u8TriggerIO, char* szString);
		void TriggerModeToString(uint8_t u8Mode, char* szString);
		
		unsigned char m_u8RecBuffer[CINSTRUCTIONDECODER_RECBUFFERSIZE];
		int m_i16RecBufferPointer;
		boolean m_bSTXReceived;
		boolean m_bFrameReceived;
		unsigned char m_szInstruction[16];
		unsigned char m_szParameter[CINSTRUCTIONDECODER_MAX_PARAMETERS][64];
		
		void PrintError(uint8_t u8Error);
#ifndef CCONFIG_DOCKLIGHT_MODE
		void PrintData(char* szData);
		void PrintData(bool bData);
		void PrintData(int8_t i8Data);
		void PrintData(uint8_t u8Data);
		void PrintData(int32_t i32Data);
		void PrintData(uint32_t u32Data);
		void PrintData(uint32_t u32Data[], uint8_t u8Size);
		void PrintData(float fData);
		void PrintNOK(void);
		void PrintOK(void);
#endif /* CCONFIG_DOCKLIGHT_MODE */
		
};	

extern CInstructionDecoder ID;
//-------------------------------------------------------------------
#endif /* __CINSTRUCTIONDECODER_H__ */	
//-------------------------------------------------------------------
