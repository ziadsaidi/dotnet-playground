namespace Sales.Application.Common.Mapping;

public interface IMapper<TSource, TDestination>
{
  TDestination Map(TSource source);
}