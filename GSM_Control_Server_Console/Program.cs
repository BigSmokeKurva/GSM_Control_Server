using HttpServerLite;
using System.IO.Ports;
using System.Text;
using System.Text.RegularExpressions;
using HttpMethod = HttpServerLite.HttpMethod;

namespace Program
{
    public class GSMControl
    {
        Dictionary<int, string> operators = new()
        {
            { 41201, "AWCC" },
            { 41220, "ROSHAN" },
            { 41240, "MTN Afghanistan" },
            { 41250, "Etisalat Afghanistan" },
            { 27601, "A M C  MOBIL" },
            { 27602, "Vodafone" },
            { 27603, "Eagle Mobile" },
            { 27604, "PLUS" },
            { 21303, "MOBILAND" },
            { 36251, "Telcell N.V." },
            { 36269, "Digicel Netherlands Antilles" },
            { 36291, "UTS Wireless Curacao" },
            { 722070, "Movistar" },
            { 722310, "CLARO ARGENTINA" },
            { 722341, "Personal" },
            { 28301, "ARMGSM" },
            { 28305, "MTS Armenia" },
            { 28310, "Orange" },
            { 50501, "Telstra MobileNet" },
            { 50502, "YES OPTUS" },
            { 50503, "Vodafone" },
            { 23201, "A1" },
            { 23203, "T-Mobile Austria" },
            { 23205, "Orange" },
            { 23212, "Orange" },
            { 23207, "Tele-ring" },
            { 40001, "AZERCELL GSM" },
            { 40002, "Bakcell LLC" },
            { 40004, "Nar Mobile" },
            { 52811, "DTSCom" },
            { 25701, "VELCOM" },
            { 25702, "MTS" },
            { 25704, "life:)" },
            { 20601, "PROXIMUS" },
            { 20610, "Mobistar" },
            { 20620, "KPN Group Belgium" },
            { 61602, "ETISALAT BENIN" },
            { 61603, "MTN" },
            { 61604, "Bell Benin Communication BBCOM" },
            { 61605, "GloBenin" },
            { 73601, "Nuevatel PCS De Bolivia" },
            { 73602, "Entel" },
            { 73603, "TELECEL BOLIVIA" },
            { 21803, "ERONET" },
            { 21805, "m:tel" },
            { 21890, "BH Mobile" },
            { 72403, "TIM BRASIL" },
            { 72402, "TIM BRASIL" },
            { 72404, "TIM BRASIL" },
            { 72405, "Claro" },
            { 72406, "Vivo" },
            { 72410, "Vivo" },
            { 72411, "Vivo" },
            { 72415, "Sercomtel Celular S/A" },
            { 72416, "Oi" },
            { 72423, "Vivo" },
            { 72424, "Oi" },
            { 72431, "Oi" },
            { 72434, "CTBC Cellular" },
            { 72433, "CTBC Cellular" },
            { 72432, "CTBC Cellular" },
            { 64201, "Econet Wireless Burundi" },
            { 64202, "AFRICELL PLC COMPANY" },
            { 64203, "ONATEL" },
            { 64207, "Smart Mobile" },
            { 64282, "U-COM Burundi" },
            { 62401, "MTN Cameroon" },
            { 62402, "Orange" },
            { 302340, "Execulink Telecom" },
            { 302370, "FIDO" },
            { 302380, "DMTS Mobility" },
            { 30266, "MTS" },
            { 30272, "Rogers Wireless" },
            { 302770, "RuralCom" },
            { 302940, "Wightman Telecom" },
            { 62501, "CVMOVEL" },
            { 62502, "UNITEL T+ TELECOMUNICACOES" },
            { 62301, "ETISALAT" },
            { 62302, "TELECEL CENTRAFRIQUE" },
            { 62304, "Azur-RCA" },
            { 46000, "CHINA MOBILE" },
            { 46001, "CHINA UNICOM GSM" },
            { 46002, "CHINA MOBILE" },
            { 732101, "Claro" },
            { 732111, "Tigo" },
            { 732103, "Tigo" },
            { 732123, "Movistar" },
            { 65401, "HURI" },
            { 54801, "Telecom Cook Islands" },
            { 61202, "ETISALAT" },
            { 61203, "Orange CI" },
            { 61204, "Comium Ivory Coast INC." },
            { 61205, "MTN" },
            { 36801, "ETECSA" },
            { 28001, "Cytamobile-Vodafone" },
            { 28010, "MTN" },
            { 23001, "T-Mobile CZ" },
            { 23002, "O2 - CZ" },
            { 23003, "Vodafone" },
            { 63801, "Evatis" },
            { 366020, "Digicel Dominica" },
            { 366110, "LIME" },
            { 37001, "ORANGE" },
            { 37002, "CLARO GSM" },
            { 37004, "Trilogy Dominicana" },
            { 63001, "VODACOM CONGO" },
            { 63002, "Airtel Congo DRC" },
            { 63086, "Orange RDC" },
            { 63088, "YTT" },
            { 63089, "TIGO" },
            { 63090, "Africell RDC" },
            { 74000, "MOVISTAR" },
            { 74001, "Claro Ecuador" },
            { 60201, "Mobinil" },
            { 60202, "Vodafone" },
            { 60203, "Etisalat" },
            { 24801, "EMT" },
            { 24802, "Elisa Eesti" },
            { 24803, "Tele2" },
            { 63601, "ETMTN" },
            { 55001, "FSM Telecommunications Corporation" },
            { 24403, "DNA LTD" },
            { 24412, "DNA LTD" },
            { 24405, "Elisa Corporation" },
            { 24414, "Alands Telekommunikation Ab" },
            { 24491, "SONERA" },
            { 20802, "Orange France" },
            { 20810, "SFR" },
            { 20813, "SFR (Contact)" },
            { 20820, "Bouygues Telecom" },
            { 20888, "Bouygues Telecom (Contact)" },
            { 34001, "Orange" },
            { 34020, "DIGICEL F" },
            { 54720, "VINI" },
            { 62801, "LIBERTIS" },
            { 62802, "ETISALAT GABON" },
            { 62803, "Airtel" },
            { 28201, "GEOCELL" },
            { 28202, "Magticom LTD" },
            { 28204, "Mobitel" },
            { 62001, "MTN" },
            { 62002, "Vodafone Ghana" },
            { 62003, "tiGO" },
            { 62006, "Airtel" },
            { 62007, "Glo Ghana" },
            { 23410, "O2" },
            { 23415, "Vodafone" },
            { 23419, "PMN" },
            { 23430, "T-Mobile" },
            { 23431, "T-Mobile" },
            { 23432, "T-Mobile" },
            { 23433, "Orange" },
            { 23450, "JT" },
            { 23455, "Cable & Wireless Guernsey Ltd (SURE)" },
            { 23458, "Manx Telecom" },
            { 90114, "AeroMobile" },
            { 73801, "Digicel Guyana" },
            { 73802, "Cellink Plus" },
            { 45400, "CSL" },
            { 45402, "CSL" },
            { 45418, "CSL" },
            { 45404, "3" },
            { 45406, "SmarTone HK" },
            { 45415, "SmarTone HK" },
            { 45417, "SmarTone HK" },
            { 45410, "CSL" },
            { 45412, "China Mobile HK" },
            { 45413, "China Mobile HK" },
            { 45416, "HKT" },
            { 45419, "HKT" },
            { 45420, "HKT" },
            { 21601, "Telenor" },
            { 21630, "T-Mobile Hungary" },
            { 21670, "Vodafone" },
            { 27401, "Siminn" },
            { 27412, "Siminn" },
            { 27402, "Vodafone Iceland" },
            { 27404, "Viking wireless" },
            { 27408, "NE - On-Waves" },
            { 40401, "Vodafone - Haryana" },
            { 40402, "airtel" },
            { 40403, "airtel" },
            { 404030, "Vodafone - Kolkata" },
            { 40404, "Idea Cellular Ltd" },
            { 40405, "Vodafone - Gujarat" },
            { 40407, "Idea Cellular Ltd" },
            { 40409, "RELIANCE TELECOM LIMITED" },
            { 40410, "airtel" },
            { 40411, "Vodafone - Delhi" },
            { 40412, "Idea Cellular Ltd" },
            { 40413, "Vodafone - Andhra Pradesh" },
            { 40414, "Idea Cellular Ltd" },
            { 40415, "Vodafone - UP East" },
            { 40416, "AIRTEL" },
            { 40417, "Aircel" },
            { 40418, "RELIANCE TELECOM LIMITED" },
            { 40419, "Idea Cellular Ltd" },
            { 40420, "Vodafone - Mumbai" },
            { 40421, "Loop Mobile" },
            { 40422, "Idea Cellular Ltd" },
            { 40424, "Idea Cellular Ltd" },
            { 40425, "Aircel" },
            { 40427, "Vodafone - Maharashtra & Goa" },
            { 40428, "Aircel" },
            { 40429, "Aircel" },
            { 40431, "airtel" },
            { 40433, "Aircel" },
            { 40434, "CellOne Haryana" },
            { 40435, "Aircel" },
            { 40436, "RELIANCE TELECOM LIMITED" },
            { 40437, "Aircel" },
            { 40438, "CellOne Assam" },
            { 40440, "airtel" },
            { 40441, "Aircel Cellular Ltd." },
            { 40442, "AIRCEL" },
            { 40443, "Vodafone - Tamil Nadu" },
            { 40444, "Idea Cellular Ltd" },
            { 40445, "airtel" },
            { 40446, "Vodafone - Kerala" },
            { 40449, "airtel" },
            { 40450, "RELIANCE TELECOM LIMITED" },
            { 40451, "CellOne Himachal Pradesh" },
            { 40452, "RELIANCE TELECOM LIMITED" },
            { 40453, "CellOne Punjab" },
            { 40454, "CellOne Uttar Pradesh (West)" },
            { 40455, "CellOne Uttar Pradesh (East)" },
            { 40456, "Idea Cellular Ltd" },
            { 40457, "CellOne Gujarat" },
            { 40458, "CellOne Madhya Pradesh" },
            { 40459, "CellOne Rajasthan" },
            { 40460, "Vodafone - Rajasthan" },
            { 40462, "CellOne Jammu & Kashmir" },
            { 40464, "CellOne Chennai" },
            { 40466, "CellOne Maharashtra" },
            { 40467, "RELIANCE TELECOM LIMITED" },
            { 40468, "Mahanagar Telephone Nigam" },
            { 40469, "Mahanagar Telephone Nigam" },
            { 40470, "AIRTEL" },
            { 40471, "CellOne Karnataka" },
            { 40472, "CellOne Kerala" },
            { 40473, "CellOne Andhra Pradesh" },
            { 40474, "CellOne West Bengal" },
            { 40475, "CellOne Bihar" },
            { 40476, "CellOne Orissa" },
            { 40477, "CellOne North East" },
            { 40478, "Idea Cellular Ltd" },
            { 40479, "CellOne All India except Delhi & Mumbai" },
            { 40480, "CellOne Tamil Nadu " },
            { 40481, "CellOne Kolkata" },
            { 40482, "Idea Cellular Ltd" },
            { 40483, "RELIANCE TELECOM LTD" },
            { 40484, "Vodafone - Chennai" },
            { 40485, "RELIANCE TELECOM LIMITED" },
            { 40486, "Vodafone - Karnataka" },
            { 40487, "Idea Cellular Ltd" },
            { 40488, "Vodafone - Punjab" },
            { 40489, "Idea Cellular Ltd" },
            { 40490, "airtel" },
            { 40491, "Aircel" },
            { 40492, "airtel" },
            { 40493, "airtel" },
            { 40494, "airtel" },
            { 40495, "airtel" },
            { 40496, "airtel" },
            { 40497, "airtel" },
            { 40498, "airtel" },
            { 40501, "Reliance Communications Limited" },
            { 405025, "TATA TELESERVICES LTD" },
            { 405027, "TATA TELESERVICES LTD" },
            { 405029, "TATA TELESERVICES LTD" },
            { 405030, "TATA TELESERVICES LTD" },
            { 405031, "TATA TELESERVICES LTD" },
            { 405032, "TATA TELESERVICES LTD" },
            { 405034, "TATA TELESERVICES LTD" },
            { 405035, "TATA TELESERVICES LTD" },
            { 405036, "TATA TELESERVICES LTD" },
            { 405037, "Tata TeleServices(Maharashtra) Limited" },
            { 405038, "TATA TELESERVICES LTD" },
            { 405039, "Tata TeleServices(Maharashtra) Limited" },
            { 405041, "TATA TELESERVICES LTD" },
            { 405042, "TATA TELESERVICES LTD" },
            { 405043, "TATA TELESERVICES LTD" },
            { 405044, "TATA TELESERVICES LTD" },
            { 405045, "TATA TELESERVICES LTD" },
            { 405046, "TATA TELESERVICES LTD" },
            { 405047, "TATA TELESERVICES LTD" },
            { 40505, "Reliance Communications Limited" },
            { 40506, "Reliance Communications Limited" },
            { 40507, "Reliance Communications Limited" },
            { 40509, "Reliance Communications Limited" },
            { 40510, "Reliance Communications Limited" },
            { 40511, "Reliance Communications Limited" },
            { 40513, "Reliance Communications Limited" },
            { 40515, "Reliance Communications Limited" },
            { 40518, "Reliance Communications Limited" },
            { 40519, "Reliance Communications Limited" },
            { 40520, "Reliance Communications Limited" },
            { 40521, "Reliance Communications Limited" },
            { 40522, "Reliance Communications Limited" },
            { 40551, "airtel" },
            { 40552, "airtel" },
            { 40553, "airtel" },
            { 40554, "airtel" },
            { 40555, "airtel" },
            { 40556, "airtel" },
            { 40566, "Vodafone - UP West" },
            { 40567, "Vodafone - West Bengal" },
            { 40570, "Idea Cellular Ltd" },
            { 405750, "Vodafone JAMMU & KASHMIR" },
            { 405751, "Vodafone ASSAM" },
            { 405752, "Vodafone BIHAR" },
            { 405753, "Vodafone ORISSA" },
            { 405754, "Vodafone HIMACHAL PRADESH" },
            { 405755, "Vodafone NORTHEAST" },
            { 405756, "Vodafone MADHYA PRADESH" },
            { 405799, "Idea Cellular Ltd" },
            { 405800, "Aircel" },
            { 405801, "Aircel" },
            { 405802, "Aircel" },
            { 405803, "Aircel " },
            { 405804, "Aircel" },
            { 405805, "Aircel" },
            { 405806, "Aircel" },
            { 405807, "Aircel" },
            { 405808, "Aircel" },
            { 405809, "Aircel" },
            { 405810, "Aircel" },
            { 405811, "Aircel" },
            { 405812, "Aircel" },
            { 405813, "uninor" },
            { 405814, "uninor" },
            { 405815, "uninor" },
            { 405816, "uninor" },
            { 405817, "uninor" },
            { 405818, "uninor" },
            { 405819, "uninor" },
            { 405820, "uninor" },
            { 405821, "uninor" },
            { 405822, "uninor" },
            { 405823, "Datacom Solutions Pvt. Ltd." },
            { 405824, "Datacom Solutions Pvt. Ltd." },
            { 405825, "Datacom Solutions Pvt. Ltd." },
            { 405827, "Datacom Solutions Pvt. Ltd." },
            { 405828, "Datacom Solutions Pvt. Ltd." },
            { 405829, "Datacom Solutions Pvt. Ltd." },
            { 405830, "Datacom Solutions Pvt. Ltd." },
            { 405831, "Datacom Solutions Pvt. Ltd." },
            { 405832, "Datacom Solutions Pvt. Ltd." },
            { 405833, "Datacom Solutions Pvt. Ltd." },
            { 405834, "Datacom Solutions Pvt. Ltd." },
            { 405835, "Datacom Solutions Pvt. Ltd." },
            { 405836, "Datacom Solutions Pvt. Ltd." },
            { 405837, "Datacom Solutions Pvt. Ltd." },
            { 405838, "Datacom Solutions Pvt. Ltd." },
            { 405839, "Datacom Solutions Pvt. Ltd." },
            { 405840, "Datacom Solutions Pvt. Ltd." },
            { 405841, "Datacom Solutions Pvt. Ltd." },
            { 405842, "Datacom Solutions Pvt. Ltd." },
            { 405843, "Datacom Solutions Pvt. Ltd." },
            { 405844, "uninor" },
            { 405845, "Idea Cellular Ltd" },
            { 405846, "Idea Cellular Ltd" },
            { 405848, "Idea Cellular Ltd" },
            { 405849, "Idea Cellular Ltd" },
            { 405850, "Idea Cellular Ltd" },
            { 405852, "Idea Cellular Ltd" },
            { 405853, "Idea Cellular Ltd" },
            { 405854, "Loop Cellular - Andhra Pradesh" },
            { 405855, "Loop Cellular - Assam" },
            { 405856, "Loop Cellular - Bihar" },
            { 405857, "Loop Cellular - Delhi" },
            { 405858, "Loop Cellular - Gujarat" },
            { 405859, "Loop Cellular - Haryana" },
            { 405860, "Loop Cellular - Himachal Pradesh" },
            { 405861, "Loop Cellular - Jammu & Kashmir" },
            { 405862, "Loop Cellular -  Karnataka" },
            { 405863, "Loop Cellular - Kerala" },
            { 405864, "Loop Cellular - Kolkata" },
            { 405865, "Loop Cellular - Madhya Pradesh" },
            { 405866, "Loop Cellular - Maharashtra" },
            { 405867, "Loop Cellular - North East" },
            { 405868, "Loop Cellular - Orissa" },
            { 405869, "Loop Cellular - Punjab" },
            { 405870, "Loop Cellular - Rajasthan" },
            { 405871, "Loop Cellular - Tamil Nadu & Chennai" },
            { 405872, "Loop Cellular - Uttar Pradesh (East)" },
            { 405873, "Loop Cellular - Uttar Pradesh (West)" },
            { 405874, "Loop Cellular - West Bengal" },
            { 405875, "uninor" },
            { 405876, "uninor" },
            { 405877, "uninor" },
            { 405878, "uninor" },
            { 405879, "uninor" },
            { 405880, "uninor" },
            { 405925, "uninor" },
            { 405926, "uninor" },
            { 405927, "uninor" },
            { 405928, "uninor" },
            { 405929, "uninor" },
            { 41805, "Asiacell" },
            { 41820, "zain IQ" },
            { 41830, "Zain Iraq" },
            { 41840, "Korek Telecom" },
            { 27201, "vodafone" },
            { 27202, "o2 IRL" },
            { 27203, "Meteor" },
            { 42501, "Orange IL" },
            { 42502, "Cellcom Israel" },
            { 42505, "Palestine Telecommunications Co. P.L.C" },
            { 2221, "TIM" },
            { 22210, "vodafone" },
            { 22288, "Wind Telecomunicazioni SpA" },
            { 90126, "Telecom Italia" },
            { 338050, "Digicel Jamaica" },
            { 338180, "LIME" },
            { 41601, "Zain JO" },
            { 41603, "Umniah" },
            { 41677, "Orange" },
            { 40101, "Beeline" },
            { 40102, "KCELL" },
            { 40177, "Mobile Telecom Service" },
            { 63902, "Safaricom" },
            { 63903, "Airtel Kenya" },
            { 63907, "Orange Kenya" },
            { 54509, "Kiribati Frigate" },
            { 43701, "Beeline KG" },
            { 43705, "MEGACOM" },
            { 43709, "O!" },
            { 45701, "LAO TELECOMMUNICATIONS" },
            { 45702, "ETL MOBILE" },
            { 45703, "LAT" },
            { 45708, "Beeline" },
            { 61801, "Lonestar Cell" },
            { 61807, "Cellcom" },
            { 29501, "Swisscom" },
            { 29502, "Orange FL" },
            { 29505, "FL1" },
            { 24601, "OMNITEL" },
            { 24602, "BITE GSM" },
            { 24603, "TELE2" },
            { 27001, "LUXGSM" },
            { 27077, "TANGO" },
            { 27099, "Orange" },
            { 29401, "T-Mobile Macedonia" },
            { 29402, "ONE" },
            { 29403, "VIP MKD" },
            { 47201, "Dhiraagu" },
            { 47202, "Wataniya Telecom Maldives Pvt. Ltd" },
            { 61001, "MALITEL" },
            { 61002, "Orange MALI" },
            { 27801, "vodafone" },
            { 27821, "go mobile" },
            { 334020, "TELCEL GSM" },
            { 33403, "movistar" },
            { 334050, "Iusacell GSM" },
            { 25901, "VoXtel" },
            { 25902, "MOLDCELL" },
            { 29702, "Telekom.me" },
            { 60401, "IAM" },
            { 64301, "mcel" },
            { 64303, "MOVITEL" },
            { 64304, "Vodacom Mozambique" },
            { 64901, "MTC" },
            { 64903, "Leo" },
            { 53001, "vodafone" },
            { 53024, "2degrees" },
            { 62120, "AirtelNG" },
            { 24201, "TELENOR" },
            { 24202, "NetCom" },
            { 24205, "Mobile Norway" },
            { 41001, "Mobilink" },
            { 41003, "Ufone" },
            { 41004, "ZONG" },
            { 41006, "Telenor Pakistan (Pvt) Ltd." },
            { 41007, "Warid Telecom" },
            { 55201, "PalauCel" },
            { 55280, "Palau Mobile" },
            { 71401, "Cable & Wireless Panama" },
            { 714020, "Movistar" },
            { 71403, "Claro Panama" },
            { 71404, "Digicel Panama" },
            { 53701, "Bee Mobile" },
            { 53703, "Digicel PNG" },
            { 71606, "Movistar Peru" },
            { 71610, "CLARO PER" },
            { 71615, "VIETTEL PERU S.A.C" },
            { 26001, "Plus" },
            { 26002, "T-Mobile.pl" },
            { 26003, "Orange" },
            { 26006, "P4" },
            { 42701, "Q-Tel" },
            { 42702, "Vodafone Qatar" },
            { 22601, "Vodafone RO" },
            { 22603, "COSMOTE" },
            { 22610, "ORANGE" },
            { 25001, "MTS" },
            { 25002, "MegaFon" },
            { 25003, "Rostelecom OJSC" },
            { 25005, "Yeniseytelecom" },
            { 25007, "SMARTS" },
            { 25012, "Baykalwestcom" },
            { 25015, "SMARTS-Ufa" },
            { 25016, "New Telephone Company" },
            { 25017, "ROSTELECOM" },
            { 25019, "INDIGO" },
            { 25020, "TELE2" },
            { 25035, "MOTIV" },
            { 25037, "KODOTEL" },
            { 25038, "ROSTELECOM" },
            { 25039, "ROSTELECOM (RUSUT)" },
            { 25099, "Beeline" },
            { 63510, "MTN Rwanda" },
            { 63513, "TIGO RWANDA" },
            { 63514, "Airtel Rwanda Limited" },
            { 358050, "Digicel (St Lucia)" },
            { 358110, "LIME" },
            { 29266, "San Marino Telecom" },
            { 62601, "CSTmovel" },
            { 60801, "Alize" },
            { 60802, "SENTEL" },
            { 60803, "Expresso Senegal" },
            { 22001, "Telenor" },
            { 22003, "Telekom Srbija" },
            { 22005, "Vip" },
            { 61901, "Airtel Sierra Leone" },
            { 61904, "COMIUM" },
            { 61905, "Africell" },
            { 23101, "Orange SK" },
            { 23102, "Telekom" },
            { 23106, "O2 - SK" },
            { 63701, "Telesom" },
            { 63704, "SOMAFONE" },
            { 63720, "Somnet TELECOM Inc" },
            { 63730, "Golis" },
            { 63771, "SOMTEL" },
            { 21401, "Vodafone" },
            { 21403, "Orange" },
            { 21407, "Movistar" },
            { 41301, "Mobitel" },
            { 41305, "airtel" },
            { 4132, "DIALOG" },
            { 4133, "ETISALAT" },
            { 74602, "TELESUR.GSM" },
            { 74603, "Digicel Suriname NV" },
            { 74604, "Intelsur N.V." },
            { 65310, "Swazi MTN" },
            { 24001, "TeliaSonera" },
            { 24008, "Telenor" },
            { 24024, "Tele 2 AB / Telenor Sverige AB" },
            { 41701, "SYRIATEL" },
            { 41702, "MTN Syria" },
            { 43601, "TCELL" },
            { 43602, "TCELL" },
            { 43603, "MegaFon Tajikistan" },
            { 43604, "Babilon-M" },
            { 43605, "BEELINE TJ" },
            { 52001, "AIS GSM" },
            { 52018, "DTAC" },
            { 52023, "GSM 1800" },
            { 52099, "True Move" },
            { 51402, "Timor Telecom" },
            { 60501, "Orange Tunisie" },
            { 60503, "TUNISIANA" },
            { 60502, "TUNTEL" },
            { 28601, "Turkcell Iletisim Hizmetleri" },
            { 28602, "Vodafone Turkey" },
            { 28603, "AVEA" },
            { 43801, "MTS Turkmenistan" },
            { 43802, "Altyn Asyr" },
            { 64101, "Airtel Uganda" },
            { 64110, "MTN-UGANDA" },
            { 64111, "Uganda Telecom" },
            { 64114, "ORANGE UGANDA LIMITED" },
            { 64122, "Warid Telecom Uganda" },
            { 25501, "MTS UKR" },
            { 25502, "Beeline UA" },
            { 25503, "KYIVSTAR" },
            { 25506, "life:)" },
            { 25505, "GOLDEN TELECOM" },
            { 42402, "ETISALAT" },
            { 310020, "Union Telephone Company" },
            { 310032, "IT&E Wireless" },
            { 310080, "Corr Wireless Communications" },
            { 310100, "Plateau Wireless" },
            { 310140, "GTA Mpulse" },
            { 310150, "AT&T Mobility" },
            { 310170, "AT&T Mobility" },
            { 310410, "AT&T Mobility" },
            { 310380, "AT&T Mobility" },
            { 310160, "T-Mobile USA" },
            { 310190, "NE - Dutch Harbor" },
            { 310200, "T-Mobile USA" },
            { 310210, "T-Mobile USA" },
            { 310220, "T-Mobile USA" },
            { 310230, "T-Mobile USA" },
            { 310240, "T-Mobile USA" },
            { 310250, "T-Mobile USA" },
            { 310260, "T-Mobile USA" },
            { 310270, "T-Mobile USA" },
            { 310290, "NEP Wireless" },
            { 310300, "Big Sky Mobile" },
            { 310310, "T-Mobile USA" },
            { 310320, "CellularOne" },
            { 310340, "WestLink Communications" },
            { 310390, "Cellular One of East Texas" },
            { 310400, "i CAN_GSM" },
            { 310420, "Cincinnati Bell Wireless" },
            { 310450, "Viaero Wireless" },
            { 310460, "NewCore Wireless, LLC" },
            { 310470, "DOCOMO PACIFIC, INC" },
            { 310490, "T-Mobile USA" },
            { 310530, "i wireless" },
            { 310770, "i wireless" },
            { 310570, "Cellular One" },
            { 310580, "T-Mobile USA" },
            { 310590, "Verizon Wireless" },
            { 310630, "CellularOne of Texoma" },
            { 310640, "Airadigm Communications" },
            { 310650, "Jasper" },
            { 310660, "T-Mobile USA" },
            { 310690, "Immix Wireless" },
            { 310710, "Artic Slope Telephone Association Cooperative" },
            { 310730, "U.S.Cellular" },
            { 310740, "OTZ Cellular" },
            { 310800, "T-Mobile USA" },
            { 310840, "telna Mobile" },
            { 310870, "PACE" },
            { 310880, "Advantage" },
            { 310890, "Verizon Wireless" },
            { 311030, "Indigo Wireless" },
            { 311040, "Commnet" },
            { 311080, "Pine Cellular" },
            { 311090, "Long Lines Wireless" },
            { 311150, "Via Wireless" },
            { 311170, "AT&T Mobility" },
            { 311410, "AT&T Mobility" },
            { 311380, "AT&T Mobility" },
            { 311180, "West Central Wireless" },
            { 311190, "Cellular One of East Central Illinois" },
            { 311330, "Bug Tussel Wireless" },
            { 311370, "GCI Communications Corp." },
            { 311530, "NewCore Wireless LLC" },
            { 311540, "Proximiti Mobility" },
            { 311730, "Proximiti Mobility" },
            { 311680, "GreenFly" },
            { 311710, "Northeast Wireless Networks" },
            { 312060, "Vanu Coverage Co" },
            { 43404, "Beeline" },
            { 43405, "Ucell" },
            { 43407, "Uzdunrobita GSM" },
            { 73402, "DIGITEL GSM" },
            { 73404, "Movistar" },
            { 73406, "Telecomunicaciones Movilnet" },
            { 42101, "Yemen Company for Mobile Telephony" },
            { 42102, "MTN" },
            { 421700, "Y" },
            { 60301, "Mobilis" },
            { 60302, "Djezzy" },
            { 60303, "Nedjma" },
            { 544110, "BLUESKY" },
            { 63102, "UNITEL" },
            { 365840, "LIME" },
            { 344030, "APUA imobile" },
            { 344920, "LIME" },
            { 344930, "Digicel Antigua & Barbuda" },
            { 36301, "SETAR GSM" },
            { 36320, "Digicel" },
            { 36439, "The Bahamas Telecommunications Company Ltd" },
            { 42601, "BATELCO" },
            { 42602, "Zain BH" },
            { 42604, "VIVA" },
            { 47001, "Grameenphone" },
            { 47002, "Robi" },
            { 47003, "Banglalink" },
            { 47004, "Teletalk Bangladesh Ltd" },
            { 47007, "Airtel" },
            { 342600, "LIME" },
            { 342750, "Digicel" },
            { 70267, "Belize Telecommunications" },
            { 350000, "CELLONE" },
            { 350010, "Digicel Bermuda" },
            { 40211, "B-Mobile" },
            { 40277, "TashiCell" },
            { 65201, "MASCOM" },
            { 65202, "ORANGE" },
            { 65204, "beMOBILE" },
            { 348170, "LIME" },
            { 348570, "CCT" },
            { 28401, "Mobiltel EAD" },
            { 28403, "Vivacom" },
            { 28405, "GLOBUL" },
            { 61302, "Airtel" },
            { 45601, "MOBITEL" },
            { 45602, "Latelz Co Ltd." },
            { 45604, "CADCOMMS" },
            { 45605, "SMART MOBILE" },
            { 45606, "SMART MOBILE" },
            { 45608, "Metfone" },
            { 45609, "Beeline-KH" },
            { 45618, "Mfone" },
            { 346140, "LIME" },
            { 62201, "Airtel Chad" },
            { 62203, "Millicom Tchad" },
            { 62207, "SALAM" },
            { 73001, "ENTEL PCS" },
            { 73003, "CLARO CHILE" },
            { 73007, "Telefonica Movil de Chile" },
            { 73010, "ENTEL PCS" },
            { 71201, "I.C.E." },
            { 71202, "I.C.E." },
            { 71203, "Claro CR" },
            { 71204, "Movistar" },
            { 21901, "T-Mobile HR" },
            { 21902, "Tele2" },
            { 21910, "VIP-NET" },
            { 23801, "TDC Mobil" },
            { 23802, "Telenor DK" },
            { 23877, "Telenor DK" },
            { 23820, "TeliaSonera DK" },
            { 23866, "Telia-Telenor DK" },
            { 70601, "CLARO SLV" },
            { 70602, "Digicel" },
            { 70603, "TIGO" },
            { 70604, "Telefonica" },
            { 62701, "Orange GQ" },
            { 62703, "HiTs-GE" },
            { 750001, "Cable and Wireless" },
            { 28801, "Faroese Telecom" },
            { 28802, "VODAFONE FO" },
            { 54201, "VODAFONE" },
            { 54202, "Digicel (Fiji) Limited" },
            { 54715, "Vodafone" },
            { 60701, "GAMCELL" },
            { 60702, "AFRICELL" },
            { 60703, "Comium Gambia" },
            { 60704, "Qcell" },
            { 26201, "Telekom Deutschland GmbH" },
            { 26202, "Vodafone" },
            { 26203, "E-Plus" },
            { 26207, "O2 (Germany)" },
            { 26208, "O2 (Germany)" },
            { 26601, "GIBTEL" },
            { 26609, "Shine" },
            { 20201, "COSMOTE" },
            { 20205, "Vodafone" },
            { 20210, "WIND" },
            { 20209, "WIND" },
            { 29001, "TELE Greenland" },
            { 352030, "Digicel" },
            { 352110, "LIME" },
            { 70401, "CLARO" },
            { 70402, "COMCEL GUATEMALA" },
            { 70403, "Telefonica" },
            { 61101, "Orange Guinee" },
            { 61102, "LAGUI" },
            { 61104, "MTN" },
            { 61105, "Cellcom Guinee" },
            { 63202, "MTN" },
            { 63203, "Orange Bissau" },
            { 37201, "Comcel" },
            { 37203, "NATCOM" },
            { 708001, "CLARO GSM" },
            { 70802, "CELTEL" },
            { 708030, "Empresa Hondurena de Telecomunicaciones" },
            { 51001, "INDOSAT" },
            { 51008, "AXIS" },
            { 51010, "TELKOMSEL" },
            { 51011, "XL" },
            { 51021, "INDOSAT" },
            { 51089, "3" },
            { 43211, "MCI" },
            { 43214, "TKC" },
            { 43232, "Taliya" },
            { 43235, "MTN Irancell" },
            { 23403, "Airtel-Vodafone" },
            { 42506, "Wataniya Mobile" },
            { 41902, "zain KW" },
            { 41903, "Wataniya Telecom" },
            { 41904, "VIVA KUWAIT" },
            { 24701, "LMT" },
            { 24702, "TELE2" },
            { 24705, "Bite Latvija" },
            { 41501, "Alfa" },
            { 41503, "Touch" },
            { 41505, "Ogero Mobile (OM)" },
            { 65101, "VODACOM LESOTHO" },
            { 65102, "Econet Telecom Lesotho (Pty) Ltd " },
            { 60600, "Libyana Mobile Phone" },
            { 60601, "MADAR" },
            { 45500, "SmarTone" },
            { 45501, "CTM" },
            { 45503, "3 Macau" },
            { 64601, "airtel" },
            { 64602, "Orange Madagascar" },
            { 64604, "Telma Mobile" },
            { 65001, "TNM" },
            { 65010, "Airtel Malawi" },
            { 50212, "MMS & MB" },
            { 50216, "DiGi" },
            { 50219, "CELCOM GSM" },
            { 34008, "DAUPHIN" },
            { 34002, "Outremer" },
            { 34003, "Tel Cell" },
            { 60901, "MATTEL" },
            { 60902, "Chinguitel" },
            { 60910, "MAURITEL" },
            { 61701, "ORANGE MRU" },
            { 61710, "EMTEL" },
            { 61703, "MTML" },
            { 21201, "Monaco Telecom" },
            { 42888, "Unitel" },
            { 42899, "MobiCom" },
            { 354860, "LIME" },
            { 60400, "MEDITEL" },
            { 41401, "MPT GSM Network" },
            { 42902, "Ncell" },
            { 20404, "vodafone" },
            { 20412, "Telfort B.V." },
            { 20416, "T-Mobile NL" },
            { 20408, "KPN B.V." },
            { 54601, "MOBILIS" },
            { 71021, "CLARO NIC" },
            { 71073, "CLARO NIC" },
            { 710300, "movistarNI" },
            { 61402, "Airtel" },
            { 61403, "ETISALAT" },
            { 61404, "Orange Niger" },
            { 62130, "MTN Nigeria" },
            { 62150, "Glo Mobile" },
            { 62160, "EMTS" },
            { 50510, "Norfolk Telecom" },
            { 42202, "OMAN MOBILE" },
            { 42203, "nawras" },
            { 74401, "VOX" },
            { 74402, "CLARO PARAGUAY" },
            { 74404, "Telecel Paraguay" },
            { 74405, "Personal" },
            { 51502, "Globe Telecom" },
            { 51503, "SMART Gold" },
            { 51505, "Digitel Mobile/Sun Cellular" },
            { 26801, "vodafone" },
            { 26803, "OPTIMUS" },
            { 26806, "TMN" },
            { 330110, "Claro GSM" },
            { 62902, "Azur Congo" },
            { 62907, "Warid Congo" },
            { 62910, "Libertis Telecom" },
            { 63005, "Supercell" },
            { 64700, "Orange Reunion" },
            { 64702, "OUTREMER TELECOM" },
            { 64710, "SFR REUNION" },
            { 356110, "LIME" },
            { 30801, "AMERIS" },
            { 360070, "Digicel (St. Vincent and Grenadines)" },
            { 360110, "LIME" },
            { 54900, "Digicel" },
            { 54927, "Bluesky Samoa Limited" },
            { 42001, "STC" },
            { 42003, "Mobily" },
            { 42004, "Zain Saudi Arabia" },
            { 22002, "Telenor" },
            { 63310, "AIRTEL" },
            { 63301, "CABLE & WIRELESS" },
            { 52501, "SingTel" },
            { 52502, "SingTel-G18" },
            { 52503, "M1 Limited" },
            { 52505, "STARHUB" },
            { 29340, "SI.mobil d.d" },
            { 29341, "MOBITEL" },
            { 29370, "Tusmobil" },
            { 54002, "Bemobile" },
            { 54001, "BREEZE" },
            { 65501, "VodaCom" },
            { 65502, "8.ta" },
            { 65507, "Cell C" },
            { 65510, "MTN-SA" },
            { 65902, "MTN South Sudan" },
            { 65903, "Gemtel Ltd" },
            { 65904, "Vivacell" },
            { 41308, "Hutchison Telecommunications Lanka (Pte)" },
            { 63401, "Zain Sudan" },
            { 63402, "MTN" },
            { 65906, "Sudanese Mobile Telephone (ZAIN) CO.LTD" },
            { 22801, "Swisscom" },
            { 22802, "Sunrise" },
            { 22803, "Orange" },
            { 22815, "ONAIR" },
            { 46601, "Far EasTone" },
            { 46688, "Far EasTone" },
            { 46692, "Chunghwa Telecom" },
            { 46693, "MOBITAI" },
            { 46697, "Taiwan Mobile" },
            { 64002, "Tigo" },
            { 64004, "Vodacom Tanzania Limited" },
            { 64005, "Airtel Tanzania" },
            { 64008, "Smart Mobile" },
            { 64003, "ZANTEL" },
            { 61501, "TOGO CELL" },
            { 61503, "ETISALAT" },
            { 53901, "U-CALL" },
            { 53988, "Digicel (Tonga) Limited" },
            { 37412, "TSTT" },
            { 376350, "LIME" },
            { 42403, "du" },
            { 74801, "Antel" },
            { 74807, "MOVISTAR" },
            { 74810, "CLARO URUGUAY" },
            { 54101, "SMILE" },
            { 54105, "Digicel" },
            { 45201, "Mobifone" },
            { 45202, "VINAPHONE" },
            { 45204, "Viettel Mobile" },
            { 45205, "Vietnamobile" },
            { 45207, "Gmobile" },
            { 64501, "Airtel Zambia" },
            { 64502, "MTN ZAMBIA" },
            { 64801, "Net*One Cellular" },
            { 64803, "TELECEL" },
            { 64804, "ECONET" },
        };
        // ComNumber, imsi, operator, gsm info, phone, availability
        public Dictionary<string, Dictionary<string, dynamic>> ports = new();
        Regex gsmRegex = new(@"[^a-zA-Z0-9 - : |]");
        Regex fromRegex = new("\",\"(.*?)\",\"");
        Regex receivedTimeRegex = new(@"\d (.*?)\+");
        // Match
        Regex messageRegex = new(@"^[\d](.*)");
        // group
        Regex sendedRegex = new(",\"(.*?)\",");
        Regex phoneNumberRegex = new(":(.*).");
        Regex cNumRegex = new(",\"(.*?)\",");
        string HexToString(string hex) => Encoding.BigEndianUnicode.GetString(Convert.FromHexString(hex));
        string GetOperator(string imsi)
        {
            foreach (var operatorNum in operators.Keys)
            {
                if (imsi.StartsWith(operatorNum.ToString()))
                {
                    return operators[operatorNum];
                }
            }
            return "Operator not found";
        }
        string SendCommand(SerialPort port, string command, int timeout)
        {
            // Я не знаю каким способом, но это пофиксило один баг
            while (port.ReadExisting().Trim().Length != 0)
            {
                port.DiscardOutBuffer();
                port.DiscardInBuffer();
                port.BaseStream.Flush();
                Thread.Sleep(700);
            }
            string answer = "";
            port.WriteLine(command);
            for (var i = 0; i < timeout / 100; i++)
            {
                Thread.Sleep(100);
                answer = port.ReadExisting().Replace("OK", "").Replace(command, string.Empty).Trim();
                if (answer.Length > 0)
                {
                    break;
                }
            }
            return answer;
        }
        public void Start()
        {
            var portNum = 1;
            foreach (string portName in SerialPort.GetPortNames())
            {
                using var port = new SerialPort(portName, 115200, Parity.None, 8, StopBits.One);
                port.Open();
                var gsmInfo = gsmRegex.Replace(SendCommand(port: port, command: "ATI", timeout: 3000).Replace("\n", "|"), string.Empty);
                if (gsmInfo.Length < 5)
                {
                    continue;
                }
                // Текстовый режим
                SendCommand(port: port, command: "AT+CMGF=1", timeout: 1500);
                /*var imsi = SendCommand(port: port, command: "AT+CIMI", timeout: 3000);
                if (imsi == "ERROR")
                {
                    imsi = "Not found";
                }*/
                //var _operator = GetOperator(imsi);

                ports[$"Port {portNum}"] = new()
                {
                    { "comName", portName },
                    { "imsi", string.Empty },
                    { "operator", string.Empty },
                    { "gsmInfo", gsmInfo },
                    { "phoneNumber", string.Empty },
                    { "availability", false },
                    { "messages", new List<string>() }
                };
                port.Close();
                portNum++;
            }
            foreach (var portName in ports.Keys) new Thread(() => ThreadPortChecker(int.Parse(portName.Split(" ")[1]))).Start();

        }
        public void ThreadPortChecker(int portNum)
        {
            string answer;
            string _operator;
            using var port = new SerialPort(ports[$"Port {portNum}"]["comName"], 115200, Parity.None, 8, StopBits.One);

            Console.WriteLine($"Port {portNum} ({ports[$"Port {portNum}"]["comName"]}) started.");
            while (true)
            {
                try
                {
                    Thread.Sleep(2300);
                    if(!port.IsOpen) port.Open();

                    var imsi = SendCommand(port, "AT+CIMI", 3000);
                    if (imsi.Length < 5)
                    {
                        ports[$"Port {portNum}"]["imsi"] = string.Empty;
                        ports[$"Port {portNum}"]["operator"] = string.Empty;
                        ports[$"Port {portNum}"]["phoneNumber"] = string.Empty;
                        ports[$"Port {portNum}"]["availability"] = false;
                        continue;
                    }
                    _operator = GetOperator(imsi);
                    // avab = false | imsi !=
                    if (!ports[$"Port {portNum}"]["availability"] || ports[$"Port {portNum}"]["imsi"] != imsi || ports[$"Port {portNum}"]["phoneNumber"].Length == 0)
                    {
                        ports[$"Port {portNum}"]["imsi"] = string.Empty;
                        ports[$"Port {portNum}"]["operator"] = string.Empty;
                        ports[$"Port {portNum}"]["phoneNumber"] = string.Empty;
                        ports[$"Port {portNum}"]["availability"] = false;
                        // Удалить все сообщения
                        SendCommand(port, "AT+CMGD=2,4", 1000);
                        // Получить номер
                        if (_operator == "MTS")
                        {

                            //SendCommand(port: port, command: "AT+CMGF=1", timeout: 1500);
                            answer = SendCommand(port, "AT+CUSD=1,\"*111*0887#\",15", 7000);
                            var answerMessage = HexToString(sendedRegex.Match(answer).Groups[1].Value);
                            if (!answerMessage.Contains("Запрос отправлен")) continue;
                            for (var i = 0; i < 80; i++)
                            {
                                if (ports[$"Port {portNum}"]["phoneNumber"] != string.Empty) break;
                                Thread.Sleep(1000);
                                answer = SendCommand(port, "AT+CMGL=\"ALL\"", 7000);
                                foreach (var message in answer.Split("+CMGL"))
                                {
                                    var from = fromRegex.Match(message).Groups[1].Value;
                                    if (from != "111") continue;
                                    var _message = HexToString(message.Split("\n")[1].Trim());
                                    if (!_message.Contains("Ваш номер")) continue;
                                    var phoneNumber = phoneNumberRegex.Match(_message).Groups[1].Value;
                                    ports[$"Port {portNum}"]["imsi"] = imsi;
                                    ports[$"Port {portNum}"]["operator"] = _operator;
                                    ports[$"Port {portNum}"]["phoneNumber"] = phoneNumber;
                                    ports[$"Port {portNum}"]["availability"] = true;
                                    break;
                                }
                                SendCommand(port, "AT+CMGD=2,1", 1000);
                            }
                        }
                        else if (_operator == "Operator not found") continue;
                        else
                        {
                            answer = SendCommand(port, "AT+CNUM", 3000);
                            var phoneNumber = cNumRegex.Match(answer).Groups[1].Value;
                            ports[$"Port {portNum}"]["imsi"] = imsi;
                            ports[$"Port {portNum}"]["operator"] = _operator;
                            ports[$"Port {portNum}"]["phoneNumber"] = phoneNumber;
                            ports[$"Port {portNum}"]["availability"] = true;
                        }
                        // Удалить все сообщения
                        ports[$"Port {portNum}"]["messages"].Clear();
                        // Для мтс не всегда работает
                        SendCommand(port, "AT+CMGD=2,4", 1000);
                    }
                    // Получение смс
                    answer = SendCommand(port, "AT+CMGL=\"ALL\"", 7000);
                    foreach (var message in answer.Split("+CMGL"))
                    {
                        if (message.Length == 0) continue;
                        string from;
                        string _message;
                        string receivedTime;
                        from = fromRegex.Match(message).Groups[1].Value;
                        _message = HexToString(message.Split("\n")[1].Replace("\n", string.Empty).Trim());
                        receivedTime = receivedTimeRegex.Match(message).Groups[1].Value;
                        ports[$"Port {portNum}"]["messages"].Add($"{receivedTime}|{from}|{_message}");
                    }
                    SendCommand(port, "AT+CMGD=2,1", 1000);
                }
                catch
                {
                }
            }
        }
    }
    class WebServer
    {
        public GSMControl gsmControlClass = Program.gsmControlClass;
        Webserver server;

        public void Start()
        {
            server = new Webserver(Program.ip, Program.port, false, null, null, DefaultRoute);
            server.Start();
            Console.WriteLine("WebServer started!");
        }
        // http://localhost:9710
        async Task DefaultRoute(HttpContext ctx)
        {
            string resp = "Нахуй пошел, тебе тут не место.";
            ctx.Response.ContentType = "text/html; charset=utf-8";
            var bresponse = Encoding.UTF8.GetBytes(resp);
            ctx.Response.ContentLength = bresponse.Length;
            await ctx.Response.SendAsync(bresponse);
        }

        // http://localhost:9710/tyujt55hftghj56esdggj5yfgbn5dfg/GetPorts
        [ParameterRoute(HttpMethod.GET, "/{apikey}/GetPorts")]
        public async Task GetPortsRoute(HttpContext ctx)
        {
            ctx.Response.StatusCode = 200;
            ctx.Response.ContentType = "application/json; charset=utf-8";
            string response;
            if (ctx.Request.Url.Parameters["apikey"] != Program.api)
            {
                response = "Invalid apikey";
            }
            else
            {
                string json = "{\n";
                foreach (var key in gsmControlClass.ports.Keys)
                {
                    json += "\t\"" + key + "\": {\n";
                    foreach (var key1 in gsmControlClass.ports[key].Keys)
                    {
                        if (key1 == "messages") continue;
                        else if (key1 == "availability")
                        {
                            json += "\t\t\"" + key1 + "\": " + (gsmControlClass.ports[key][key1] ? "true" : "false") + "\n";
                            continue;
                        }
                        json += "\t\t\"" + key1 + "\": \"" + gsmControlClass.ports[key][key1] + "\",\n";
                    }
                    json += key != gsmControlClass.ports.Keys.Last() ? "\t},\n" : "\t}\n";
                }
                json += "}";
                response = json;
            }
            var bresponse = Encoding.UTF8.GetBytes(response);
            ctx.Response.ContentLength = bresponse.Length;
            await ctx.Response.SendAsync(bresponse);
            return;
        }

        // http://localhost:9710/tyujt55hftghj56esdggj5yfgbn5dfg/GetMsgs/Port/1
        [ParameterRoute(HttpMethod.GET, "/{apikey}/GetMsgs/Port/{portNum}")]
        public async Task GetMsgsRoute(HttpContext ctx)
        {
            ctx.Response.StatusCode = 200;
            ctx.Response.ContentType = "application/json; charset=utf-8";
            string response;
            if (ctx.Request.Url.Parameters["apikey"] != Program.api)
            {
                response = "Invalid apikey";
            }
            else
            {
                response = "";
                if (gsmControlClass.ports[$"Port {ctx.Request.Url.Parameters["portNum"]}"]["messages"].Count == 0) response = "No messages";
                else
                {
                    foreach (string message in gsmControlClass.ports[$"Port {ctx.Request.Url.Parameters["portNum"]}"]["messages"])
                    {
                        response += message + "\n";
                    }
                }
            }
            var bresponse = Encoding.UTF8.GetBytes(response);
            ctx.Response.ContentLength = bresponse.Length;
            await ctx.Response.SendAsync(bresponse);
            return;
        }

        // http://localhost:9000/tyujt55hftghj56esdggj5yfgbn5dfg/ClearMsgs/Port/1
        [ParameterRoute(HttpMethod.GET, "/{apikey}/ClearMsgs/Port/{portNum}")]
        public async Task ClearMsgsRoute(HttpContext ctx)
        {
            ctx.Response.StatusCode = 200;
            ctx.Response.ContentType = "application/json; charset=utf-8";
            string response;
            if (ctx.Request.Url.Parameters["apikey"] != Program.api)
            {
                response = "Invalid apikey";
            }
            else
            {
                response = "";
                gsmControlClass.ports[$"Port {ctx.Request.Url.Parameters["portNum"]}"]["messages"].Clear();
            }
            var bresponse = Encoding.UTF8.GetBytes(response);
            ctx.Response.ContentLength = bresponse.Length;
            await ctx.Response.SendAsync(bresponse);
            return;
        }
    }
    public class Program
    {
        static public GSMControl gsmControlClass = new();
        static WebServer webserver = new();
        public static readonly string ip = "0.0.0.0";
        public static readonly int port = 9710;
        public static readonly string api = "rty7u467rtuty4567tyj45y";
        public static void Main()
        {
            gsmControlClass.Start();
            webserver.Start();

            while (true)
            {
                Thread.Sleep(2000);
                Console.Clear();
                Console.WriteLine($"Count ports: {gsmControlClass.ports.Values.Count}.");
                for (var i = 1; i <= gsmControlClass.ports.Values.Count; i++)
                {
                    Console.WriteLine($"Port {i} ({gsmControlClass.ports[$"Port {i}"]["comName"]}) - availability: {gsmControlClass.ports[$"Port {i}"]["availability"]}; messagesCount: {gsmControlClass.ports[$"Port {i}"]["messages"].Count}; phoneNumber: {gsmControlClass.ports[$"Port {i}"]["phoneNumber"]}.");
                }
                Console.WriteLine($"Web server started - {ip}:{port}.");
            }
        }
    }
}
