using Application.Interfaces;

using Domain.Models;

using Infrastructure.DbContexts;

namespace Infrastructure.Repository;

internal class FileDataRepository : BaseRepository<FileData>, IFileDataRepository
{
    public FileDataRepository(CommandDbContext commandDbContext, QueryDbContext queryDbContext) : base(commandDbContext, queryDbContext)
    {
    }

    public async Task<FileData> AddDataAsync(FileData fileData, CancellationToken cancellationToken)
    {
        await AddAsync(fileData, cancellationToken);

        await commandDbContext.SaveChangesAsync(cancellationToken);

        return fileData;
    }

    public async Task<FileData?> GetFileDataByIdAsync(long fileId, CancellationToken cancellationToken) =>
        await GetByIdAsync(fileId, cancellationToken);
}
