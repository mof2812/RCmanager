/*
 * RelayCard.ino
 *
 * Created: 2/22/2022 9:44:45 AM
 * Author: Michael
 */
#include <Arduino.h>
#include "CConfig.h"
#include "CDebug.h"
#include "CTimer.h"
#include "CTrigger.h"
#include "CInstructionDecoder.h"
#include "TimerOne.h"
#ifdef CCONFIG_DOCKLIGHT_MODE
	#include "CVersion.h"
#endif /* CCONFIG_DOCKLIGHT_MODE */
 
void setup()
{
/* add setup code here, setup code runs once when the processor starts */
	Debug.Init();
	Config.Init();
	ID.Init();
	Timer.Init();
	Trigger.Init();
#ifndef _DEBUG
	Timer1.initialize((1000*CCONFIG_TIMERINTERVAL));		/* timerinterval of timer interrupt */
	Timer1.attachInterrupt(TimerIRQ);
#endif /* _DEBUG */	
	delay(2000);
#ifdef CCONFIG_DOCKLIGHT_MODE
	ID.ClearScreen();
	CCONFIG_SERIAL_COM.println();
	CCONFIG_SERIAL_COM.println("Relaiskarten-Steuerung");
	CCONFIG_SERIAL_COM.println("======================");
	CCONFIG_SERIAL_COM.print("Version: ");
	CCONFIG_SERIAL_COM.println(CVERSION_SW_VERSION);
	CCONFIG_SERIAL_COM.println("von Michael Offenbach");
	CCONFIG_SERIAL_COM.println();
#endif /* CCONFIG_DOCKLIGHT_MODE */

}

void loop()
{
	static uint32_t u32CycleTime;
	 
/* add main program code here, this code starts again each time it ends */
	u32CycleTime = millis();

	ID.Task();
#ifdef _DEBUG
	Timer.Task();
#endif /* _DEBUG */
	Trigger.Task();
	
	u32CycleTime = millis() - u32CycleTime;
	
}

void TimerIRQ(void)
{
	Timer.Task();
	
}
