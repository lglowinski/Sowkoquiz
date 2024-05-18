using MediatR;
using Sowkoquiz.Application.Common;
using Sowkoquiz.Domain.ActiveQuizEntity;

namespace Sowkoquiz.Application.Quiz.DeleteUserQuiz;

public class DeleteUserQuizCommandHandler(IActiveQuizRepository activeQuizRepository) : IRequestHandler<DeleteUserQuizCommand, bool>
{
    public async Task<bool> Handle(DeleteUserQuizCommand request, CancellationToken cancellationToken)
    {
        var quiz = await activeQuizRepository.FindByIdAsync(request.Id, cancellationToken);

        if (quiz is null || !quiz.HasAccess(request.AccessKey))
            return false;

        quiz.Status = QuizStatus.Deleted;
        
        return await activeQuizRepository.UpdateAsync(quiz, cancellationToken);
    }
}