﻿namespace StayHard.Dto;

public class ScoreDto
{
    public int QuestionId { get; set; }
    public int StudentId { get; set; }
    public int Number { get; set; }
    public int ExamId { get; set; }
    public float AvailableScore { get; set; }
    public float TakenScore { get; set; }
}