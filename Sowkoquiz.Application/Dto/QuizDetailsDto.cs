namespace Sowkoquiz.Application.Dto;

public record QuizDetailsDto(
    int Id,
    string Name,
    List<AnsweredQuestionDto> AnsweredQuestionDtos,
    ProgressDto ProgressDto,
    DateTimeOffset Date);

public record AnsweredQuestionDto(int Id, string Text, string Answer, string Correct, bool IsCorrect);

public record ProgressDto(int Correct, int Total);