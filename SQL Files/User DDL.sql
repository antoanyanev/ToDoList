﻿use Data;

create table Users (
	id int not null identity primary key,
	Name varchar(255) not null,
	Surname varchar(255) not null,
	Birthdate varchar(255) not null,
	Gender varchar(255) not null,
	City varchar(255) not null
)