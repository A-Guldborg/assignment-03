namespace Assignment3.Entities;

public class TagRepository : ITagRepository
{
    private readonly KanbanContext _context;

    public TagRepository(KanbanContext context) {
        _context = context;
    }

    public (Response Response, int TagId) Create(TagCreateDTO tag)
    {
        var entity = _context.Tags.FirstOrDefault(t => t.Name == tag.Name);

        Response response;

        if (entity is null) {
            entity = new Tag(tag.Name);
            
            _context.Tags.Add(entity);
            _context.SaveChanges();

            response = Response.Created;
        } else {
            response = Response.Conflict;
        }

        return (response, entity.Id);
    }

    public IReadOnlyCollection<TagDTO> ReadAll()
    {
        var collection = from t in _context.Tags
                         select new TagDTO(t.Id, t.Name);
                        
        return collection.ToList();
    }

    public TagDTO Read(int tagId)
    {
        var entity = from t in _context.Tags
                    where t.Id == tagId
                    select new TagDTO(t.Id, t.Name);

        return entity.FirstOrDefault();
    }

    public Response Update(TagUpdateDTO tag)
    {
        var entity = _context.Tags.FirstOrDefault(t => t.Id == tag.Id);
        if (entity is null) return Response.NotFound;

        entity.Name = tag.Name;
        _context.SaveChanges();
        return Response.Updated;
    }

    public Response Delete(int tagId, bool force = false)
    {
        var entity = _context.Tags.FirstOrDefault(t => t.Id == tagId);

        if (entity is null) return Response.NotFound;

        if (!force && entity.Tasks.Any())
        {
            return Response.Conflict;
        } 

        _context.Remove(entity);
        _context.SaveChanges();
        return Response.Deleted;
    }
}
