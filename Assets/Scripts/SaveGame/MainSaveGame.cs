﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wth.ModApi;

[Serializable]
public class MainSaveGame : SaveGame
{
	public string name { get; set; }
	public string modId { get; set; }
	public DateTime saveDate { get; set; }
	
	public List<EmployeeData> employeesForHire { get; set; }
	public List<EmployeeData> employeesHired { get; set; }
}
