/*
 * Help.c
 *
 * Created: 22.10.2017 09:51:04
 *  Author: Michael
 */ 

/* Includes */
#include <arduino.h>
#include "Help.h"
#include "CConfig.h"

#define TAB "    "
#define MTAB(A) {int i; for(i=0;i<A;i++){CCONFIG_SERIAL_COM.print(F(TAB));}}
#define PRINT_TAB(A) {CCONFIG_SERIAL_COM.print(F(TAB)); CCONFIG_SERIAL_COM.print(F(A));}
#define PRINT_TAB(A) {CCONFIG_SERIAL_COM.print(F(TAB)); CCONFIG_SERIAL_COM.print(F(A));}
#define PRINT_TAB_LN(A) {CCONFIG_SERIAL_COM.print(F(TAB)); CCONFIG_SERIAL_COM.println(F(A));}
#define PRINT_MTAB(A,B) {int i; for(i=0;i<A;i++){CCONFIG_SERIAL_COM.print(F(TAB));} CCONFIG_SERIAL_COM.print(F(B));}
#define PRINT_MTAB_LN(A,B) {int i; for(i=0;i<A;i++){CCONFIG_SERIAL_COM.print(F(TAB));} CCONFIG_SERIAL_COM.println(F(B));}

#define PRINT_CAPTION(A) {CCONFIG_SERIAL_COM.print(F(A)); CCONFIG_SERIAL_COM.println(F(":"));}
#define PRINT_COMMAND(A,B,C,D,E,T) \ 
{\
	CCONFIG_SERIAL_COM.print(F(TAB)); CCONFIG_SERIAL_COM.print(F("<")); CCONFIG_SERIAL_COM.print(F(A)); CCONFIG_SERIAL_COM.print(F(">:"));\ 
	if(strlen(B) > 0) {CCONFIG_SERIAL_COM.print(F("  <")); CCONFIG_SERIAL_COM.print(F(B)); CCONFIG_SERIAL_COM.print(F(">"));}\ 
	if(strlen(C) > 0) {CCONFIG_SERIAL_COM.print(F("  <")); CCONFIG_SERIAL_COM.print(F(C)); CCONFIG_SERIAL_COM.print(F(">"));}\  
	if(strlen(D) > 0) {CCONFIG_SERIAL_COM.print(F("  <")); CCONFIG_SERIAL_COM.print(F(D)); CCONFIG_SERIAL_COM.print(F(">"));}\  
	if(strlen(E) > 0) {CCONFIG_SERIAL_COM.print(F("  <")); CCONFIG_SERIAL_COM.print(F(E)); CCONFIG_SERIAL_COM.print(F(">"));}\ 
	if(strlen(T) > 0) {CCONFIG_SERIAL_COM.print(F("  ")); CCONFIG_SERIAL_COM.print(F(T));}\ 
	CCONFIG_SERIAL_COM.println(F(""));\
}
#define PRINT_COMMAND_EX(A,B,C,D,E,G,H,I,J,K,L,M,N,T) \
{\
	CCONFIG_SERIAL_COM.print(F(TAB)); CCONFIG_SERIAL_COM.print(F("<")); CCONFIG_SERIAL_COM.print(F(A)); CCONFIG_SERIAL_COM.print(F(">:"));\
	if(strlen(B) > 0) {CCONFIG_SERIAL_COM.print(F("  <")); CCONFIG_SERIAL_COM.print(F(B)); CCONFIG_SERIAL_COM.print(F(">"));}\
	if(strlen(C) > 0) {CCONFIG_SERIAL_COM.print(F("  <")); CCONFIG_SERIAL_COM.print(F(C)); CCONFIG_SERIAL_COM.print(F(">"));}\
	if(strlen(D) > 0) {CCONFIG_SERIAL_COM.print(F("  <")); CCONFIG_SERIAL_COM.print(F(D)); CCONFIG_SERIAL_COM.print(F(">"));}\
	if(strlen(E) > 0) {CCONFIG_SERIAL_COM.print(F("  <")); CCONFIG_SERIAL_COM.print(F(E)); CCONFIG_SERIAL_COM.print(F(">"));}\
	if(strlen(G) > 0) {CCONFIG_SERIAL_COM.print(F("  <")); CCONFIG_SERIAL_COM.print(F(G)); CCONFIG_SERIAL_COM.print(F(">"));}\
	if(strlen(H) > 0) {CCONFIG_SERIAL_COM.print(F("  <")); CCONFIG_SERIAL_COM.print(F(H)); CCONFIG_SERIAL_COM.print(F(">"));}\
	if(strlen(I) > 0) {CCONFIG_SERIAL_COM.print(F("  <")); CCONFIG_SERIAL_COM.print(F(I)); CCONFIG_SERIAL_COM.print(F(">"));}\
	if(strlen(J) > 0) {CCONFIG_SERIAL_COM.print(F("  <")); CCONFIG_SERIAL_COM.print(F(J)); CCONFIG_SERIAL_COM.print(F(">"));}\
	if(strlen(K) > 0) {CCONFIG_SERIAL_COM.print(F("  <")); CCONFIG_SERIAL_COM.print(F(K)); CCONFIG_SERIAL_COM.print(F(">"));}\
	if(strlen(L) > 0) {CCONFIG_SERIAL_COM.print(F("  <")); CCONFIG_SERIAL_COM.print(F(L)); CCONFIG_SERIAL_COM.print(F(">"));}\
	if(strlen(M) > 0) {CCONFIG_SERIAL_COM.print(F("  <")); CCONFIG_SERIAL_COM.print(F(M)); CCONFIG_SERIAL_COM.print(F(">"));}\
	if(strlen(N) > 0) {CCONFIG_SERIAL_COM.print(F("  <")); CCONFIG_SERIAL_COM.print(F(N)); CCONFIG_SERIAL_COM.print(F(">"));}\
	if(strlen(T) > 0) {CCONFIG_SERIAL_COM.print(F("  ")); CCONFIG_SERIAL_COM.print(F(T));}\
	CCONFIG_SERIAL_COM.println(F(""));\
}
/*	
#define PRINT_COMMAND_EX(A,B,C,D,E,G,H,I,J,K,L,M,N,T) \
{\
	CCONFIG_SERIAL_COM.print(F(TAB)); CCONFIG_SERIAL_COM.print(F("<")); CCONFIG_SERIAL_COM.print(F(A)); CCONFIG_SERIAL_COM.print(F(">:"));\
	if(strlen(B) > 0) {CCONFIG_SERIAL_COM.print(F("  <")); CCONFIG_SERIAL_COM.print(F(B)); CCONFIG_SERIAL_COM.print(F(">"));}\
	if(strlen(C) > 0) {CCONFIG_SERIAL_COM.print(F("  <")); CCONFIG_SERIAL_COM.print(F(C)); CCONFIG_SERIAL_COM.print(F(">"));}\
	if(strlen(D) > 0) {CCONFIG_SERIAL_COM.print(F("  <")); CCONFIG_SERIAL_COM.print(F(D)); CCONFIG_SERIAL_COM.print(F(">"));}\
	if(strlen(E) > 0) {CCONFIG_SERIAL_COM.print(F("  <")); CCONFIG_SERIAL_COM.print(F(E)); CCONFIG_SERIAL_COM.print(F(">"));}\
	if(strlen(F) > 0) {CCONFIG_SERIAL_COM.print(F("  <")); CCONFIG_SERIAL_COM.print(F(F)); CCONFIG_SERIAL_COM.print(F(">"));}\
	if(strlen(G) > 0) {CCONFIG_SERIAL_COM.print(F("  <")); CCONFIG_SERIAL_COM.print(F(G)); CCONFIG_SERIAL_COM.print(F(">"));}\
	if(strlen(H) > 0) {CCONFIG_SERIAL_COM.print(F("  <")); CCONFIG_SERIAL_COM.print(F(H)); CCONFIG_SERIAL_COM.print(F(">"));}\
	if(strlen(I) > 0) {CCONFIG_SERIAL_COM.print(F("  <")); CCONFIG_SERIAL_COM.print(F(I)); CCONFIG_SERIAL_COM.print(F(">"));}\
	if(strlen(J) > 0) {CCONFIG_SERIAL_COM.print(F("  <")); CCONFIG_SERIAL_COM.print(F(J)); CCONFIG_SERIAL_COM.print(F(">"));}\
	if(strlen(K) > 0) {CCONFIG_SERIAL_COM.print(F("  <")); CCONFIG_SERIAL_COM.print(F(K)); CCONFIG_SERIAL_COM.print(F(">"));}\
	if(strlen(L) > 0) {CCONFIG_SERIAL_COM.print(F("  <")); CCONFIG_SERIAL_COM.print(F(L)); CCONFIG_SERIAL_COM.print(F(">"));}\
	if(strlen(M) > 0) {CCONFIG_SERIAL_COM.print(F("  <")); CCONFIG_SERIAL_COM.print(F(M)); CCONFIG_SERIAL_COM.print(F(">"));}\
	if(strlen(N) > 0) {CCONFIG_SERIAL_COM.print(F("  <")); CCONFIG_SERIAL_COM.print(F(N)); CCONFIG_SERIAL_COM.print(F(">"));}\
	if(strlen(T) > 0) {CCONFIG_SERIAL_COM.print(F("  ")); CCONFIG_SERIAL_COM.print(F(T)); CCONFIG_SERIAL_COM.println(F(""));}\
}
*/
#define PRINT_PARAMETER(A,B,T) {MTAB(3) CCONFIG_SERIAL_COM.print(F("Parameter #")); CCONFIG_SERIAL_COM.print(F(A)); CCONFIG_SERIAL_COM.print(F(":  ")); CCONFIG_SERIAL_COM.print(F("<")); CCONFIG_SERIAL_COM.print(F(B)); CCONFIG_SERIAL_COM.print(F(">  ")); CCONFIG_SERIAL_COM.print(F(T));}
#define PRINT_PARAMETER_LN(A,B,T) {MTAB(3) CCONFIG_SERIAL_COM.print(F("Parameter #")); CCONFIG_SERIAL_COM.print(F(A)); CCONFIG_SERIAL_COM.print(F(":  ")); CCONFIG_SERIAL_COM.print(F("<")); CCONFIG_SERIAL_COM.print(F(B)); CCONFIG_SERIAL_COM.print(F(">  ")); CCONFIG_SERIAL_COM.println(F(T));}
// #define PRINT_COMMENT_LN(A) {CCONFIG_SERIAL_COM.println(""); MTAB(3) CCONFIG_SERIAL_COM.println(F(A)); CCONFIG_SERIAL_COM.println("");}
#define PRINT_COMMENT_LN(A) {MTAB(4) CCONFIG_SERIAL_COM.println(F(A)); CCONFIG_SERIAL_COM.println("");}

void Help();

void Help()
{
PRINT_CAPTION("C");
/*
	PRINT_COMMAND("CLR","","","","", "CLeaR all outputs");
	PRINT_COMMENT_LN("Alle Kanalausgaenge loeschen");
*/
PRINT_CAPTION("D");
/*
	PRINT_COMMAND("DSP","int","","","", "set DiSPlay mode");
	PRINT_PARAMETER_LN("1","int","Mode - 0: Statusausgabe via RS232 unterdruecken, 1: Statusausgabe via RS232 vornehmen");
	PRINT_COMMENT_LN("Statusausgabe der Kanalausgaenge ein/aus");
*/

PRINT_CAPTION("E");
/*
	PRINT_COMMAND("ENA","int","int","","", "ENAble output");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Schaltkanal)");
	PRINT_PARAMETER_LN("2","int","Freigabe (0 = gesperrt, 1 = freigegeben)");
	PRINT_COMMENT_LN("Kanalausgang freigeben");
*/

PRINT_CAPTION("G");
	PRINT_COMMAND("GCFG","int","int","","", "Get ConFiG");
	PRINT_PARAMETER_LN("1","int","Angabe über den auszugebenden Konfigurationsparameter (ENABLE)");
	PRINT_PARAMETER_LN("2","int","Wert");
	PRINT_COMMENT_LN("Konfigurationsparameter ausgeben");


	PRINT_COMMAND("GPSPAF","int","","","", "Get PowerSuPply Asynchronous Flag");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Netzteil-Schaltkanal, Bereich: 0-7)");
	PRINT_COMMENT_LN("Asynchron-Flag ausgeben");

	PRINT_COMMAND("GPSPC","int","","","", "Get PowerSuPply Counts");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Netzteil-Schaltkanal, Bereich: 0-7)");
	PRINT_COMMENT_LN("Impulse im Impulsbetrieb ausgeben");

	PRINT_COMMAND("GPSPCA","int","","","", "Get PowerSuPply Counts Actual");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Netzteil-Schaltkanal, Bereich: 0-7)");
	PRINT_COMMENT_LN("Verbleibende Impulse im Impulsbetrieb ausgeben");

	PRINT_COMMAND("GPSPCONFIG","int","","","", "Get PowerSuPply CONFIGuration");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Netzteil-Schaltkanal, Bereich: 0-7)");
	PRINT_COMMENT_LN("Kanal-Konfiguration ausgeben");

	PRINT_COMMAND("GPSPE","int","","","", "Get PowerSuPply Enable");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Netzteil-Schaltkanal, Bereich: 0-7)");
	PRINT_COMMENT_LN("Freigabe-Status eines Netzteil-Schaltkanals ausgeben");

	PRINT_COMMAND("GPSPIF","int","","","", "Get PowerSuPply Immediate Flag");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Netzteil-Schaltkanal, Bereich: 0-7)");
	PRINT_COMMENT_LN("Immediate-Flag ausgeben");

	PRINT_COMMAND("GPSPM","int","","","", "Get PowerSuPply Mode");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Netzteil-Schaltkanal, Bereich: 0-3)");
	PRINT_COMMENT_LN("Betriebsart des Netzteil-Schaltkanals ausgeben");

	PRINT_COMMAND("GPSPNF","int","","","", "Get PowerSuPply Negation Flag");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Netzteil-Schaltkanal, Bereich: 0-7)");
	PRINT_COMMENT_LN("Negations-Flag ausgeben");

	PRINT_COMMAND("GPSPT","int","","","", "Get PowerSuPply Timing");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Netzteil-Schaltkanal, Bereich: 0-7)");
	PRINT_COMMENT_LN("Timing eines Netzteil-Schaltkanals fuer den Toggle-/Impulsbetrieb ausgeben");

	PRINT_COMMAND("GPSPTF","int","","","", "Get PowerSuPply Trigger Flag");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Netzteil-Schaltkanal, Bereich: 0-7)");
	PRINT_COMMENT_LN("Trigger-Flag ausgeben");

	PRINT_COMMAND("GSWAF","int","","","", "Get SWitch Asynchronous Flag");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Schaltkanal, Bereich: 0-7)");
	PRINT_COMMENT_LN("Asynchron-Flag ausgeben");

	PRINT_COMMAND("GSWC","int","","","", "Get SWitch Counts");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Schaltkanal, Bereich: 0-7)");
	PRINT_COMMENT_LN("Impulse im Impulsbetrieb ausgeben");

	PRINT_COMMAND("GSWCA","int","","","", "Get SWitch Counts Actual");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Schaltkanal, Bereich: 0-7)");
	PRINT_COMMENT_LN("Verbleibende Impulse im Impulsbetrieb ausgeben");

	PRINT_COMMAND("GSWCONFIG","int","","","", "Get SWitch CONFIGuration");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Schaltkanal, Bereich: 0-7)");
	PRINT_COMMENT_LN("Kanal-Konfiguration ausgeben");

	PRINT_COMMAND("GSWE","int","","","", "Get SWitch Enable");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Schaltkanal, Bereich: 0-7)");
	PRINT_COMMENT_LN("Freigabe-Status eines Schaltkanals ausgeben");

	PRINT_COMMAND("GSWIF","int","","","", "Get SWitch Immediate Flag");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Schaltkanal, Bereich: 0-7)");
	PRINT_COMMENT_LN("Immediate-Flag ausgeben");

	PRINT_COMMAND("GSWM","int","","","", "Get SWitch Mode");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Schaltkanal, Bereich: 0-3)");
	PRINT_COMMENT_LN("Betriebsart des Schaltkanals ausgeben");

	PRINT_COMMAND("GSWNF","int","","","", "Get SWitch Negation Flag");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Schaltkanal, Bereich: 0-7)");
	PRINT_COMMENT_LN("Negations-Flag ausgeben");

	PRINT_COMMAND("GSWT","int","","","", "Get SWitch Timing");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Schaltkanal, Bereich: 0-7)");
	PRINT_COMMENT_LN("Timing eines Schaltkanals fuer den Toggle-/Impulsbetrieb ausgeben");

	PRINT_COMMAND("GSWTF","int","","","", "Get SWitch Trigger Flag");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Schaltkanal, Bereich: 0-7)");
	PRINT_COMMENT_LN("Trigger-Flag ausgeben");

	PRINT_COMMAND("GTRGE","int","","","", "Get TRiGer Enable");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Schaltkanal, Bereich: 0-3)");
	PRINT_COMMENT_LN("Freigabe-Status eines Trigger-Kanals ausgeben");

	PRINT_COMMAND("GTRGL","int","","","", "Get TRiGer Level");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Schaltkanal, Bereich: 0-3)");
	PRINT_COMMENT_LN("Trigger-Schwelle eines Trigger-Kanals ausgeben");

	PRINT_COMMAND("GTRGM","int","","","", "Get TRiGer Mode");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Schaltkanal, Bereich: 0-3)");
	PRINT_COMMENT_LN("Trigger-Modus eines Trigger-Kanals ausgeben");

	PRINT_COMMAND("GTRGR","int","","","", "Get TRiGer Retrigger");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Schaltkanal, Bereich: 0-3)");
	PRINT_COMMENT_LN("Retrigger-Status eines Trigger-Kanals ausgeben");

PRINT_CAPTION("H");
	PRINT_COMMAND("HELP","","","","", "HELP system");
	PRINT_COMMENT_LN("Hilfe ausgeben");

PRINT_CAPTION("I");
	PRINT_COMMAND("INFO","","","","","INFO output");
	PRINT_COMMENT_LN("Parameter der einzelnen Kanaele in Tabellenform ausgeben");

PRINT_CAPTION("S");
	PRINT_COMMAND("SCFG","int","int","","", "Set ConFiG");
	PRINT_PARAMETER_LN("1","int","Angabe über den zu setzenden Konfigurationsparameter (ENABLE)");
	PRINT_PARAMETER_LN("2","int","Wert");
	PRINT_COMMENT_LN("Konfigurationsparameter setzen");

	PRINT_COMMAND("SPSPAF","int","int","","", "Set PowerSuPply Asynchronous Flag");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Netzteil-Schaltkanal, Bereich: 0-7)");
	PRINT_PARAMETER_LN("2","int","Wert (Bereich: 0,1)");
	PRINT_COMMENT_LN("Asynchron-Flag setzen");

	PRINT_COMMAND("SPSPC","int","int","","", "Set PowerSuPply Counter");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Netzteil-Schaltkanal, Bereich: 0-7)");
	PRINT_PARAMETER_LN("2","int","Impulse");
	PRINT_COMMENT_LN("Anzahl der Impulse im Impulsbetrieb eines Netzteil-Schaltkanals setzen");

	PRINT_COMMAND_EX("SPSPCONFIG","int","bool","int","int","int","int","int","bool","bool","bool","int","bool", "Set PowerSuPply CONFIGuration");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Netzteil-Schaltkanal, Bereich: 0-7)");
	PRINT_PARAMETER_LN("2","bool","Kanal-Freigabe (0 = gesperrt, 1 = freigegeben, Bereich: 0,1)");
	PRINT_PARAMETER_LN("3","string","Betriebsmodus (OFF, ON, TOGGLE, IMPULSE)");
	PRINT_PARAMETER_LN("4","int","Einschaltzeit (ms)");
	PRINT_PARAMETER_LN("5","int","Ausschaltzeit (ms)");
	PRINT_PARAMETER_LN("6","int","Verzoegerungszeit (ms)");
	PRINT_PARAMETER_LN("7","int","Impulse");
	PRINT_PARAMETER_LN("8","bool","Wert (0 = synchrone Betriebsart, 1 = asynchrone Betriebsart, Bereich: 0,1)");
	PRINT_PARAMETER_LN("9","bool","Wert (0 = Ausgabe mit der nächsten ON-Flanke, 1 = unmittelbar Ausgabe, Bereich: 0,1)");
	PRINT_PARAMETER_LN("10","bool","Wert (0 = nicht invertierte Betriebsart, 1 = invertierte Betriebsart, Bereich: 0,1)");
	PRINT_PARAMETER_LN("11","int","Trigger-Kanal (0 = 1.Trigger-Kanal, Bereich: 0-3)");
	PRINT_PARAMETER_LN("12","bool","Wert (0 = nicht getriggerte Betriebsart, 1 = getriggerte Betriebsart, Bereich: 0,1)");
	PRINT_COMMENT_LN("Kanal-Konfiguration ausgeben");

	PRINT_COMMAND("SPSPE","int","int","","", "Set PowerSuPply Enable");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Netzteil-Schaltkanal, Bereich: 0-7)");
	PRINT_PARAMETER_LN("2","int","Freigabe-Flag (0 = gesperrt, 1 = freigegeben, Bereich: 0-1)");
	PRINT_COMMENT_LN("Freigabe-Status eines Netzteil-Schaltkanals setzen");

	PRINT_COMMAND("SPSPIF","int","int","","", "Set PowerSuPply Immediate Flag");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Netzteil-Schaltkanal, Bereich: 0-7)");
	PRINT_PARAMETER_LN("2","int","Wert (Bereich: 0,1)");
	PRINT_COMMENT_LN("Immediate-Flag setzen");

	PRINT_COMMAND("SPSPM","int","string","","", "Set PowerSuPply Mode");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Netzteil-Schaltkanal, Bereich: 0-3)");
	PRINT_PARAMETER_LN("2","string","Betriebsmodus (OFF, ON, TOGGLE, IMPULSE)");
	PRINT_COMMENT_LN("Setzen der Betriebsart des Netzteil-Schaltkanals");

	PRINT_COMMAND("SPSPNF","int","int","","", "Set PowerSuPply Negation Flag");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Netzteil-Schaltkanal, Bereich: 0-7)");
	PRINT_PARAMETER_LN("2","int","Wert (Bereich: 0,1)");
	PRINT_COMMENT_LN("Negations-Flag setzen");

	PRINT_COMMAND("SPSPT","int","int","int","int", "Set PowerSuPply Timing");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Netzteil-Schaltkanal, Bereich: 0-7)");
	PRINT_PARAMETER_LN("2","int","Einschaltzeit (ms)");
	PRINT_PARAMETER_LN("3","int","Ausschaltzeit (ms)");
	PRINT_PARAMETER_LN("4","int","Verzoegerungszeit (ms)");
	PRINT_COMMENT_LN("Timing eines Netzteil-Schaltkanals fuer den Toggle-/Impulsbetrieb setzen");

	PRINT_COMMAND("SPSPTF","int","int","","", "Set PowerSuPply Trigger Flag");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Netzteil-Schaltkanal, Bereich: 0-7)");
	PRINT_PARAMETER_LN("1","int","Trigger-Kanal (0 = 1.Trigger-Kanal, Bereich: 0-3)");
	PRINT_PARAMETER_LN("2","int","Wert (Bereich: 0,1)");
	PRINT_COMMENT_LN("Trigger-Flag setzen");

	PRINT_COMMAND("SSWAF","int","int","","", "Set SWitch Asynchronous Flag");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Schaltkanal, Bereich: 0-7)");
	PRINT_PARAMETER_LN("2","int","Wert (Bereich: 0,1)");
	PRINT_COMMENT_LN("Asynchron-Flag setzen");

	PRINT_COMMAND("SSWC","int","int","","", "Set SWitch Counter");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Schaltkanal, Bereich: 0-7)");
	PRINT_PARAMETER_LN("2","int","Impulse");
	PRINT_COMMENT_LN("Anzahl der Impulse im Impulsbetrieb eines Schaltkanals setzen");

	PRINT_COMMAND_EX("SSWCONFIG","int","bool","int","int","int","int","int","bool","bool","bool","int","bool", "Set PowerSuPply CONFIGuration");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Netzteil-Schaltkanal, Bereich: 0-7)");
	PRINT_PARAMETER_LN("2","bool","Kanal-Freigabe (0 = gesperrt, 1 = freigegeben, Bereich: 0,1)");
	PRINT_PARAMETER_LN("3","string","Betriebsmodus (OFF, ON, TOGGLE, IMPULSE)");
	PRINT_PARAMETER_LN("4","int","Einschaltzeit (ms)");
	PRINT_PARAMETER_LN("5","int","Ausschaltzeit (ms)");
	PRINT_PARAMETER_LN("6","int","Verzoegerungszeit (ms)");
	PRINT_PARAMETER_LN("7","int","Impulse");
	PRINT_PARAMETER_LN("8","bool","Wert (0 = synchrone Betriebsart, 1 = asynchrone Betriebsart, Bereich: 0,1)");
	PRINT_PARAMETER_LN("9","bool","Wert (0 = Ausgabe mit der nächsten ON-Flanke, 1 = unmittelbar Ausgabe, Bereich: 0,1)");
	PRINT_PARAMETER_LN("10","bool","Wert (0 = nicht invertierte Betriebsart, 1 = invertierte Betriebsart, Bereich: 0,1)");
	PRINT_PARAMETER_LN("11","int","Trigger-Kanal (0 = 1.Trigger-Kanal, Bereich: 0-3)");
	PRINT_PARAMETER_LN("12","bool","Wert (0 = nicht getriggerte Betriebsart, 1 = getriggerte Betriebsart, Bereich: 0,1)");
	PRINT_COMMENT_LN("Kanal-Konfiguration ausgeben");

	PRINT_COMMAND("SSWE","int","int","","", "Set SWitch Enable");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Schaltkanal, Bereich: 0-7)");
	PRINT_PARAMETER_LN("2","int","Freigabe-Flag (0 = gesperrt, 1 = freigegeben, Bereich: 0-1)");
	PRINT_COMMENT_LN("Freigabe-Status eines Schaltkanals setzen");

	PRINT_COMMAND("SSWIF","int","int","","", "Set SWitch Immediate Flag");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Schaltkanal, Bereich: 0-7)");
	PRINT_PARAMETER_LN("2","int","Wert (Bereich: 0,1)");
	PRINT_COMMENT_LN("Immediate-Flag setzen");

	PRINT_COMMAND("SSWM","int","string","","", "Set SWitch Mode");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Schaltkanal, Bereich: 0-3)");
	PRINT_PARAMETER_LN("2","string","Betriebsmodus (OFF, ON, TOGGLE, IMPULSE)");
	PRINT_COMMENT_LN("Setzen der Betriebsart des Schaltkanals");

	PRINT_COMMAND("SSWNF","int","int","","", "Set SWitch Negation Flag");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Schaltkanal, Bereich: 0-7)");
	PRINT_PARAMETER_LN("2","int","Wert (Bereich: 0,1)");
	PRINT_COMMENT_LN("Negations-Flag setzen");

	PRINT_COMMAND("SSWT","int","int","int","int", "Set SWitch Timing");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Schaltkanal, Bereich: 0-7)");
	PRINT_PARAMETER_LN("2","int","Einschaltzeit (ms)");
	PRINT_PARAMETER_LN("3","int","Ausschaltzeit (ms)");
	PRINT_PARAMETER_LN("4","int","Verzoegerungszeit (ms)");
	PRINT_COMMENT_LN("Timing eines Schaltkanals fuer den Toggle-/Impulsbetrieb setzen");

	PRINT_COMMAND("SSWTF","int","int","","", "Set SWitch Trigger Flag");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Schaltkanal, Bereich: 0-7)");
	PRINT_PARAMETER_LN("1","int","Trigger-Kanal (0 = 1.Trigger-Kanal, Bereich: 0-3)");
	PRINT_PARAMETER_LN("2","int","Wert (Bereich: 0,1)");
	PRINT_COMMENT_LN("Trigger-Flag setzen");

	PRINT_COMMAND("STRGE","int","int","","", "Set TRiGer Enable");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Schaltkanal, Bereich: 0-3)");
	PRINT_PARAMETER_LN("2","int","Freigabe-Flag (0 = gesperrt, 1 = freigegeben, Bereich: 0-1)");
	PRINT_COMMENT_LN("Freigabe-Status eines Trigger-Kanals setzen");

	PRINT_COMMAND("STRGL","int","float","","", "Set TRriGger Level");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Schaltkanal, Bereich: 0-3)");
	PRINT_PARAMETER_LN("2","float","Triggerlevel (Bereich: 0.00-5.00)");
	PRINT_COMMENT_LN("Setzen der Schaltschwelle zum Aktivieren des Triggers im Modus IRQ_DOWN oder IRQ_UP");

	PRINT_COMMAND("STRGM","int","string","","", "Set TRiGger Mode");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Schaltkanal, Bereich: 0-3)");
	PRINT_PARAMETER_LN("2","string","Triggermodus (LOW, HIGH, FALLING, RISING, LEVEL_DOWN, LEVEL_UP, IRQ_DOWN, IRQ_UP)");
	PRINT_COMMENT_LN("Setzen der Betriebsart des Triggers");

	PRINT_COMMAND("STRGR","int","int","","", "Set TRiGer Retrigger");
	PRINT_PARAMETER_LN("1","int","Kanal-Index (0 = 1.Schaltkanal, Bereich: 0-3)");
	PRINT_PARAMETER_LN("2","int","Retrigger-Flag (0 = gesperrt, 1 = freigegeben, Bereich: 0-1)");
	PRINT_COMMENT_LN("Retrigger eines Trigger-Kanals setzen");

PRINT_CAPTION("V");
	PRINT_COMMAND("VERSION","","","","", "VERSION der Software");
	PRINT_COMMENT_LN("Softwareversion ausgeben");
}
