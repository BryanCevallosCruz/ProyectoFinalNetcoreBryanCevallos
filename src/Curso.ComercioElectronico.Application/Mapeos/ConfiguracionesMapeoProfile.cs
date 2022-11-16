

using AutoMapper;
using Curso.ComercioElectronico.Application;
using Curso.ComercioElectronico.Domain;

public class ConfiguracionesMapeoProfile : Profile
{
    //TipoProductoCrearActualizarDto => TipoProducto
    //TipoProducto => TipoProductoDto

    public ConfiguracionesMapeoProfile()
    {

        CreateMap<MarcaCrearDto, Marca>();
        CreateMap<MarcaActualizarDto, Marca>();
        CreateMap<Marca, MarcaDto>();

        CreateMap<TipoProductoCrearDto, TipoProducto>();
        CreateMap<TipoProductoActualizarDto, TipoProducto>();
        CreateMap<TipoProducto, TipoProductoDto>();

        CreateMap<ProductoCrearActualizarDto, Producto>();
        CreateMap<Producto, ProductoDto>()
            .ForMember(dest => dest.Marca, opt => opt.MapFrom(src => src.Marca.Nombre))
            .ForMember(dest => dest.TipoProducto, opt => opt.MapFrom(src => src.TipoProducto.Nombre));

        CreateMap<BancoClienteCrearDto, BancoCliente>();
        CreateMap<BancoClienteActualizarDto, BancoCliente>();
        CreateMap<BancoCliente, BancoClienteDto>();

        CreateMap<ClienteCrearDto, Cliente>();
        CreateMap<ClienteActualizarDto, Cliente>();
        CreateMap<Cliente, ClienteDto>()
             .ForMember(dest => dest.BancoCliente, opt => opt.MapFrom(src => src.BancoCliente.Nombre));


        CreateMap<CarroCrearDto, Carro>();
        CreateMap<CarroActualizarDto, Carro>();
        CreateMap<Carro, CarroDto>()
            .ForMember(dest => dest.Cliente, opt => opt.MapFrom(src => src.Cliente.Nombre));

        
        CreateMap<CarroItem, CarroItemDto>()
            .ForMember(dest => dest.Producto, opt => opt.MapFrom(src => src.Producto.Nombre));
        CreateMap<CarroItemActualizarDto, CarroItem>();
        CreateMap<CarroItemCrearDto, CarroItem>();
        CreateMap<CarroItemDto, CarroItem>();
        CreateMap<CarroItemDto, CarroDto>();

        CreateMap<Carro, OrdenDto>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.Fecha, opt => opt.Ignore())
            .ForMember(x => x.Estado, opt => opt.Ignore())
            .ForMember(dest => dest.Cliente, opt => opt.MapFrom(src => src.Cliente.Nombre));
        CreateMap<CarroItem, OrdenItemDto>()
            .ForMember(dest => dest.Producto, opt => opt.MapFrom(src => src.Producto.Nombre));

        CreateMap<OrdenDto, Orden>();
        CreateMap<OrdenItemDto, OrdenItem>();
        CreateMap<OrdenCrearDto, Orden>();
        CreateMap<OrdenActualizarDto, Orden>();
        CreateMap<Orden, OrdenDto>()
            .ForMember(dest => dest.Cliente, opt => opt.MapFrom(src => src.Cliente.Nombre));


        CreateMap<OrdenItem, OrdenItemDto>()
            .ForMember(dest => dest.Producto, opt => opt.MapFrom(src => src.Producto.Nombre));
        CreateMap<OrdenItemActualizarDto, OrdenItem>();
        CreateMap<OrdenItemCrearDto, OrdenItem>();
        CreateMap<OrdenItemDto, OrdenItem>();
        CreateMap<OrdenItemDto, OrdenDto>();

    }
}

