// <auto-generated/>
#if !PUREDI_API

#if NET20 || NET35 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6
namespace System.Diagnostics.CodeAnalysis
{
    // ReSharper disable UnusedType.Global
    [System.AttributeUsage(System.AttributeTargets.Assembly | System.AttributeTargets.Class | System.AttributeTargets.Constructor | System.AttributeTargets.Event | System.AttributeTargets.Method | System.AttributeTargets.Property | System.AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public sealed class ExcludeFromCodeCoverageAttribute : Attribute
    {
    }
}
#endif

#if NET20
namespace System
{
    public delegate TResult Func<TResult>();
    public delegate TResult Func<T, TResult>(T arg);
}
#endif

namespace Pure.DI
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Binding lifetimes.
    /// </summary>
    internal enum Lifetime
    {
        /// <summary>
        /// Creates a new instance of the requested type every time.
        /// </summary>
        Transient,

        /// <summary>
        /// Creates an instance first time and then provides the same instance each time.
        /// </summary>
        Singleton,

        /// <summary>
        /// The per resolve lifetime is similar to the <see cref="Lifetime.Transient"/>, but it reuses the instance in the recursive object graph.
        /// </summary>
        PerResolve
    }
    
    internal enum Hint
    {
        /// <summary>
        /// <c>On</c> or <c>Off</c>. Determines whether to generate <c>Resolve</c> methods](#resolve-methods). <c>On</c> by default.
        /// </summary>
        Resolve,
        
        /// <summary>
        /// <c>On</c> or <c>Off</c>. Determines whether to generate partial <c>OnInstanceCreation</c> method. <c>Off</c> by default.
        /// </summary>
        OnInstanceCreation,
        
        /// <summary>
        /// The regular expression to filter OnInstanceCreation by the instance type name. ".+" by default.
        /// </summary>
        OnInstanceCreationImplementationTypeNameRegularExpression,
        
        /// <summary>
        /// The regular expression to filter OnInstanceCreation by the tag. ".+" by default.
        /// </summary>
        OnInstanceCreationTagRegularExpression,
        
        /// <summary>
        /// The regular expression to filter OnInstanceCreation by the lifetime. ".+" by default.
        /// </summary>
        OnInstanceCreationLifetimeRegularExpression,
        
        /// <summary>
        /// <c>On</c> or <c>Off</c>. Determines whether to generate partial <c>OnDependencyInjection</c> method to control of dependency injection. <c>Off</c> by default.
        /// </summary>
        OnDependencyInjection,
        
        /// <summary>
        /// The regular expression to filter OnDependencyInjection by the instance type name. ".+" by default.
        /// </summary>
        OnDependencyInjectionImplementationTypeNameRegularExpression,
        
        /// <summary>
        /// The regular expression to filter OnDependencyInjection by the resolving type name. ".+" by default.
        /// </summary>
        OnDependencyInjectionContractTypeNameRegularExpression,
        
        /// <summary>
        /// The regular expression to filter OnDependencyInjection by the tag. ".+" by default.
        /// </summary>
        OnDependencyInjectionTagRegularExpression,
        
        /// <summary>
        /// The regular expression to filter OnDependencyInjection by the lifetime. ".+" by default.
        /// </summary>
        OnDependencyInjectionLifetimeRegularExpression,
        
        /// <summary>
        /// <c>On</c> or <c>Off</c>. Determines whether to generate a partial <c>OnCannotResolve<T>(...)</c> method to handle a scenario where an instance which cannot be resolved. <c>Off</c> by default.
        /// </summary>
        OnCannotResolve,
        
        /// <summary>
        /// The regular expression to filter OnCannotResolve by the resolving type name. ".+" by default.
        /// </summary>
        OnCannotResolveContractTypeNameRegularExpression,
        
        /// <summary>
        /// The regular expression to filter OnCannotResolve by the tag. ".+" by default.
        /// </summary>
        OnCannotResolveTagRegularExpression,
        
        /// <summary>
        /// The regular expression to filter OnCannotResolve by the lifetime. ".+" by default.
        /// </summary>
        OnCannotResolveLifetimeRegularExpression,
        
        /// <summary>
        /// <c>On</c> or <c>Off</c>. Determine if the <c>ToString()</c> method should be generated. This method provides a text-based class diagram in the format mermaid. <c>Off</c> by default. 
        /// </summary>
        ToString,
        
        /// <summary>
        /// <c>On</c> or <c>Off</c>. This hint determines whether object composition will be created in a thread-safe manner. <c>On</c> by default. 
        /// </summary>
        ThreadSafe,
        
        /// <summary>
        /// Overrides modifiers of the method <c>public T Resolve<T>()</c>. "public" by default.
        /// </summary>
        ResolveMethodModifiers,
        
        /// <summary>
        /// Overrides name of the method <c>public T Resolve<T>()</c>. "Resolve" by default.
        /// </summary>
        ResolveMethodName,
        
        /// <summary>
        /// Overrides modifiers of the method <c>public T Resolve<T>(object? tag)</c>. "public" by default.
        /// </summary>
        ResolveByTagMethodModifiers,
        
        /// <summary>
        /// Overrides name of the method <c>public T Resolve<T>(object? tag)</c>. "Resolve" by default.
        /// </summary>
        ResolveByTagMethodName,
        
        /// <summary>
        /// Overrides modifiers of the method <c>public object Resolve(Type type)</c>. "public" by default.
        /// </summary>
        ObjectResolveMethodModifiers,
        
        /// <summary>
        /// Overrides name of the method <c>public object Resolve(Type type)</c>. "Resolve" by default.
        /// </summary>
        ObjectResolveMethodName,
        
        /// <summary>
        /// Overrides modifiers of the method <c>public object Resolve(Type type, object? tag)</c>. "public" by default.
        /// </summary>
        ObjectResolveByTagMethodModifiers,
        
        /// <summary>
        /// Overrides name of the method <c>public object Resolve(Type type, object? tag)</c>. "Resolve" by default.
        /// </summary>
        ObjectResolveByTagMethodName,
        
        /// <summary>
        /// Overrides modifiers of the method <c>public void Dispose()</c>. "public" by default.
        /// </summary>
        DisposeMethodModifiers,
        
        /// <summary>
        /// <c>On</c> or <c>Off</c>. Specifies whether the generated code should be formatted. This option consumes a lot of CPU resources. <c>Off</c> by default.
        /// </summary>
        FormatCode
    }

    /// <summary>
    /// Represents the generic type arguments marker. It allows creating custom generic type arguments marker like <see cref="TTS"/>, <see cref="TTDictionary{TKey,TValue}"/> and etc. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct)]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal sealed class GenericTypeArgumentAttribute : Attribute { }
    
    /// <summary>
    /// Represents an ordinal attribute overriding an injection ordinal.
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal class OrdinalAttribute : Attribute
    {
        // ReSharper disable once MemberCanBePrivate.Global
        /// <summary>
        /// The injection ordinal.
        /// </summary>
        public readonly int Ordinal;

        /// <summary>
        /// Creates an attribute instance.
        /// </summary>
        /// <param name="ordinal">The injection ordinal.</param>
        public OrdinalAttribute(int ordinal)
        {
            Ordinal = ordinal;
        }
    }

    /// <summary>
    /// Represents a tag attribute overriding an injection tag.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal class TagAttribute : Attribute
    {
        // ReSharper disable once MemberCanBePrivate.Global
        /// <summary>
        /// The injection tag.
        /// </summary>
        public readonly object Tag;

        /// <summary>
        /// Creates an attribute instance.
        /// </summary>
        /// <param name="tag">The injection tag. See also <see cref="IBinding.Tags"/></param>.
        public TagAttribute(object tag)
        {
            Tag = tag;
        }
    }

    /// <summary>
    /// Represents a dependency type attribute overriding an injection type. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal class TypeAttribute : Attribute
    {
        // ReSharper disable once MemberCanBePrivate.Global
        /// <summary>
        /// The injection type.
        /// </summary>
        public readonly Type Type;

        /// <summary>
        /// Creates an attribute instance.
        /// </summary>
        /// <param name="type">The injection type. See also <see cref="IConfiguration.Bind{T}"/> and <see cref="IBinding.Bind{T}"/>.</param>
        public TypeAttribute(Type type)
        {
            Type = type;
        }
    }
    
    internal enum CompositionKind
    {
        Public,
        Internal,
        Global
    }
    
    /// <summary>
    /// API to configure DI.
    /// </summary>
    internal interface IConfiguration
    {
        /// <summary>
        /// Starts a binding.
        /// <example>
        /// <code>
        /// static partial class Composition
        /// {
        ///   static Composition() =&gt; DI.Setup()
        ///     .Bind&lt;IBox&lt;TT&gt;&gt;().To&gt;CardboardBox&lt;TT&gt;&gt;()
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <typeparam name="T">The type of dependency to bind. Also supports generic type markers like <see cref="TT"/>, <see cref="TTList{T}"/> and others.</typeparam>
        /// <param name="tags">The optional argument specifying the tags for the specific dependency type of binding.</param>
        /// <returns>Binding configuration API.</returns>
        IBinding Bind<T>(params object[] tags);

        /// <summary>
        /// Use some DI configuration as a base by its name.
        /// <example>
        /// <code>
        /// static partial class Composition
        /// {
        ///   static Composition() =&gt; DI.Setup()
        ///     .DependsOn("MyBaseComposition");
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="baseConfigurationNames">The name of a base DI configuration.</param>
        /// <returns>DI configuration API.</returns>
        IConfiguration DependsOn(params string[] baseConfigurationNames);

        /// <summary>
        /// Determines a custom attribute overriding an injection type.
        /// <example>
        /// <code>
        /// [AttributeUsage(
        ///   AttributeTargets.Parameter
        ///   | AttributeTargets.Property
        ///   | AttributeTargets.Field)]
        /// public class MyTypeAttribute : Attribute
        /// {
        ///   public readonly Type Type;
        ///   public MyTypeAttribute(Type type) => Type = type;
        /// }
        /// static partial class Composition
        /// {
        ///   static Composition() =&gt; DI.Setup()
        ///     .TypeAttribute&lt;MyTypeAttribute&gt;();
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="typeArgumentPosition">The optional position of a type parameter in the attribute constructor. See the predefined <see cref="TypeAttribute{T}"/> attribute.</param>
        /// <typeparam name="T">The attribute type.</typeparam>
        /// <returns>DI configuration API.</returns>
        IConfiguration TypeAttribute<T>(int typeArgumentPosition = 0) where T : Attribute;

        /// <summary>
        /// Determines a tag attribute overriding an injection tag.
        /// <example>
        /// <code>
        /// [AttributeUsage(
        ///   AttributeTargets.Parameter
        ///   | AttributeTargets.Property
        ///   | AttributeTargets.Field)]
        /// public class MyTagAttribute : Attribute
        /// {
        ///   public readonly object Tag;
        ///   public MyTagAttribute(object tag) => Tag = tag;
        /// }
        /// static partial class Composition
        /// {
        ///   static Composition() =&gt; DI.Setup()
        ///     .TagAttribute&lt;MyTagAttribute&gt;();
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="tagArgumentPosition">The optional position of a tag parameter in the attribute constructor. See the predefined <see cref="TagAttribute{T}"/> attribute.</param>
        /// <typeparam name="T">The attribute type.</typeparam>
        /// <returns>DI configuration API.</returns>
        IConfiguration TagAttribute<T>(int tagArgumentPosition = 0) where T : Attribute;

        /// <summary>
        /// Determines a custom attribute overriding an injection Ordinal.
        /// <example>
        /// <code>
        /// [AttributeUsage(
        ///   AttributeTargets.Constructor
        ///   | AttributeTargets.Method
        ///   | AttributeTargets.Property
        ///   | AttributeTargets.Field)]
        /// public class MyOrdinalAttribute : Attribute
        /// {
        ///   public readonly int Ordinal;
        ///   public MyOrdinalAttribute(int ordinal) => Ordinal = ordinal;
        /// }
        /// static partial class Composition
        /// {
        ///   static Composition() =&gt; DI.Setup()
        ///     .OrdinalAttribute&lt;MyOrdinalAttribute&gt;();
        /// }
        /// </code>
        /// </example> 
        /// </summary>
        /// <param name="ordinalArgumentPosition">The optional position of parameter in the attribute constructor. 0 by default. See the predefined <see cref="OrdinalAttribute{T}"/> attribute.</param>
        /// <typeparam name="T">The attribute type.</typeparam>
        /// <returns>DI configuration API.</returns>
        IConfiguration OrdinalAttribute<T>(int ordinalArgumentPosition = 0) where T : Attribute;

        /// <summary>
        /// Overrides a default <see cref="Lifetime"/>. <see cref="Lifetime.Transient"/> is default lifetime.
        /// <example>
        /// <code>
        /// static partial class Composition
        /// {
        ///   static Composition() =&gt; DI.Setup()
        ///     .Default(Lifetime.Singleton)
        ///     .Bind&lt;ICat&gt;().To&lt;ShroedingersCat&gt;();
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="lifetime">The new default lifetime.</param>
        /// <returns>DI configuration API.</returns>
        IConfiguration DefaultLifetime(Pure.DI.Lifetime lifetime);
        
        /// <summary>
        /// Adds a resolution argument  
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <param name="tags">The optional argument specifying the tags for the argument.</param>
        /// <typeparam name="T">The argument type.</typeparam>
        /// <returns>DI configuration.</returns>
        IConfiguration Arg<T>(string name, params object[] tags);
        
        /// <summary>
        /// Specifies a composition root
        /// </summary>
        /// <param name="name">The name of the root.</param>
        /// <param name="tag">Optional argument indicating the tag for the root of the composition.</param>
        /// <typeparam name="T">The composition root type.</typeparam>
        /// <returns>DI configuration.</returns>
        IConfiguration Root<T>(string name = "", object tag = null);

        /// <summary>
        /// Determines a hint
        /// </summary>
        /// <param name="hint">The hint name.</param>
        /// <param name="value">The hint value.</param>
        /// <returns>DI configuration.</returns>
        IConfiguration Hint(Hint hint, string value);
    }

    /// <summary>
    /// API to configure a binding.
    /// </summary>
    internal interface IBinding
    {
        /// <summary>
        /// Continue a binding configuration chain, determining an additional dependency type.
        /// <example>
        /// <code>
        /// static partial class Composition
        /// {
        ///   static Composition() =&gt; DI.Setup()
        ///     .Bind&lt;ICat&gt;().To&lt;ShroedingersCat&gt;();
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <typeparam name="T">The type of dependency to bind. Also supports generic type markers like <see cref="TT"/>, <see cref="TTList{T}"/> and others.</typeparam>
        /// <param name="tags">The optional argument specifying the tags for the specific dependency type of binding.</param>
        /// <returns>Binding configuration API.</returns>
        IBinding Bind<T>(params object[] tags);

        /// <summary>
        /// Determines a binding <see cref="Lifetime"/>.
        /// <example>
        /// <code>
        /// static partial class Composition
        /// {
        ///   static Composition() =&gt; DI.Setup()
        ///     .Bind&lt;ICat&gt;().As(Lifetime.Singleton).To&lt;ShroedingersCat&gt;();
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="lifetime">The binding <see cref="Lifetime"/>.</param>
        /// <returns>Binding configuration API.</returns>
        IBinding As(Pure.DI.Lifetime lifetime);

        /// <summary>
        /// Determines a binding tag.
        /// <example>
        /// <code>
        /// static partial class Composition
        /// {
        ///   static Composition() =&gt; DI.Setup()
        ///     .Bind&lt;ICat&gt;().Tag("MyCat").To&lt;ShroedingersCat&gt;();
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="tags">Tags for all dependency types of binding.</param>
        /// <returns>Binding configuration API.</returns>
        IBinding Tags(params object[] tags);

        /// <summary>
        /// Finish a binding configuration chain by determining a binding implementation.
        /// <example>
        /// <code>
        /// static partial class Composition
        /// {
        ///   static Composition() =&gt; DI.Setup()
        ///     .Bind&lt;ICat&gt;().To&lt;ShroedingersCat&gt;();
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <typeparam name="T">The type of binding implementation. Also supports generic type markers like <see cref="TT"/>, <see cref="TTList{T}"/> and others.</typeparam>
        /// <returns>DI configuration API.</returns>
        IConfiguration To<T>();

        /// <summary>
        /// Finish a binding configuration chain by determining a binding implementation using a factory method. It allows to resole an instance manually, invoke required methods, initialize properties, fields and etc.
        /// <example>
        /// <code>
        /// DI.Setup()
        ///  .Bind&lt;IDependency&gt;().To&lt;Dependency&gt;()
        ///  .Bind&lt;INamedService&gt;().To(
        ///    ctx =&gt;
        ///    {
        ///      var service = new InitializingNamedService(ctx.Resolve&lt;IDependency&gt;());
        ///      service.Initialize("Initialized!", ctx.Resolve&lt;IDependency&gt;());
        ///      return service;
        ///    });
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="factory">The method providing an dependency implementation.</param>
        /// <typeparam name="T">The type of binding implementation. Also supports generic type markers like <see cref="TT"/>, <see cref="TTList{T}"/> and others.</typeparam>
        /// <returns>DI configuration.</returns>
        IConfiguration To<T>(Func<IContext, T> factory);
    }

    /// <summary>
    /// The abstraction to inject a DI dependency via <see cref="IBinding.To{T}(System.Func{IContext,T})"/>.
    /// </summary>
    internal interface IContext
    {
        object Tag { get; }
            
        /// <summary>
        /// Injects an instance of type <c>T</c>.
        /// </summary>
        void Inject<T>(out T value);

        /// <summary>
        /// Injects an instance of type <c>T</c> marked with a tag.
        /// <summary>
        void Inject<T>(object tag, out T value);
    }
    
    /// <summary>
    /// Provides API to configure a DI composition.
    /// <example>
    /// <code>
    /// static partial class Composition
    /// {
    ///   private static readonly Random Indeterminacy = new();
    ///   static Composition() =&gt; DI.Setup()
    ///     .Bind&lt;State&gt;().To(_ =&gt; (State)Indeterminacy.Next(2))
    ///     .Bind&lt;ICat&gt;().To&lt;ShroedingersCat&gt;()
    ///     .Bind&lt;IBox&lt;TT&gt;&gt;().To&gt;CardboardBox&lt;TT&gt;&gt;()
    ///     .Bind&lt;Program&gt;().As(Lifetime.Singleton).To&lt;Program&gt;();
    /// }
    /// </code>
    /// </example>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static class DI
    {
        /// <summary>
        /// Starts a new or continues an existing DI configuration chain.
        /// <example>
        /// <code>
        /// static partial class Composition
        /// {
        ///   static Composition() =&gt; DI.Setup("MyComposition");
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="compositionTypeName">This argument specifying a custom DI composition type name to generate. By default, it is a name of an owner class if the owner class is <c>static partial class</c> otherwise, it is a name of an owner plus the "DI" postfix. /// <param name="compositionTypeName">The optional argument specifying a custom DI composition type name to generate. By default, it is a name of an owner class if the owner class is <c>static partial class</c> otherwise, it is a name of an owner plus the "DI" postfix. For a top level statements application the name is <c>Composition</c> by default.</param></param>
        /// <param name="kind">This argument specifying a composition scope. By default, it is <c>Public</c> by default.</param></param>
        /// <returns>DI configuration API.</returns>
        [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)0x300)]
        internal static IConfiguration Setup(string compositionTypeName, CompositionKind kind = CompositionKind.Public)
        {
            return Configuration.Shared;
        }

        private sealed class Configuration : IConfiguration
        {
            public static readonly IConfiguration Shared = new Configuration();

            private Configuration() { }

            /// <inheritdoc />
            [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)0x300)]
            public IBinding Bind<T>(params object[] tags)
            {
                return Binding.Shared;
            }

            /// <inheritdoc />
            [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)0x300)]
            public IConfiguration DependsOn(params string[] baseConfigurationName)
            {
                return Configuration.Shared;
            }

            /// <inheritdoc />
            [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)0x300)]
            public IConfiguration TypeAttribute<T>(int typeArgumentPosition = 0) where T : Attribute
            {
                return Configuration.Shared;
            }

            /// <inheritdoc />
            [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)0x300)]
            public IConfiguration TagAttribute<T>(int tagArgumentPosition = 0) where T : Attribute
            {
                return Configuration.Shared;
            }

            /// <inheritdoc />
            [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)0x300)]
            public IConfiguration OrdinalAttribute<T>(int ordinalArgumentPosition = 0) where T : Attribute
            {
                return Configuration.Shared;
            }

            /// <inheritdoc />
            [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)0x300)]
            public IConfiguration DefaultLifetime(Pure.DI.Lifetime lifetime)
            {
                return Configuration.Shared;
            }
            
            /// <inheritdoc />
            [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)0x300)]
            public IConfiguration Arg<T>(string name, params object[] tags)
            {
                return Configuration.Shared;
            }

            /// <inheritdoc />
            [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)0x300)]
            public IConfiguration Root<T>(string name, object tag)
            {
                return Configuration.Shared;
            }

            /// <inheritdoc />
            [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)0x300)]
            public IConfiguration Hint(Hint hint, string value)
            {
                return Configuration.Shared;
            }
        }

        private sealed class Binding : IBinding
        {
            public static readonly IBinding Shared = new Binding();

            private Binding()
            {
            }

            /// <inheritdoc />
            [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)0x300)]
            public IBinding Bind<T>(params object[] tags)
            {
                return Shared;
            }

            /// <inheritdoc />
            [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)0x300)]
            public IBinding As(Pure.DI.Lifetime lifetime)
            {
                return Shared;
            }

            /// <inheritdoc />
            [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)0x300)]
            public IBinding Tags(params object[] tags)
            {
                return Shared;
            }

            /// <inheritdoc />
            [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)0x300)]
            public IConfiguration To<T>()
            {
                return Configuration.Shared;
            }

            /// <inheritdoc />
            [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)0x300)]
            public IConfiguration To<T>(Func<IContext, T> factory)
            {
                return Configuration.Shared;
            }
        }
    }
    
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]
    internal struct Pair<TKey, TValue>
    {
        public readonly TKey Key;
        public readonly TValue Value;

        public Pair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public override string ToString()
        {
            return Key?.ToString() ?? "empty" + " = " + Value.ToString();
        }
    }
    
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static class Buckets<TKey, TValue>
    {
        public static uint GetDivisor(uint count)
        {
            return count < 2 ? count : count << 1;
        }

        public static Pair<TKey, TValue>[] Create(
            uint divisor,
            out int bucketSize,
            Pair<TKey, TValue>[] pairs)
        {
            bucketSize = 0;
            int[] bicketSizes = new int[divisor];
            for (int i = 0; i < pairs.Length; i++)
            {
                uint bucket = ((uint)System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(pairs[i].Key)) % divisor;
                int size = bicketSizes[bucket] + 1;
                bicketSizes[bucket] = size;
                if (size > bucketSize)
                {
                    bucketSize = size;
                }
            }
            
            Pair<TKey, TValue>[] buckets = new Pair<TKey, TValue>[divisor * bucketSize];
            for (int i = 0; i < pairs.Length; i++)
            {
                uint bucket = ((uint)System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(pairs[i].Key)) % divisor;
                var index = bicketSizes[bucket] - 1;
                buckets[bucket * bucketSize + index] = pairs[i];
                bicketSizes[bucket] = index;
            }
            
            return buckets;
        }
    }

    internal interface IResolver<TComposite, out T>
    {
        T Resolve(TComposite composite);
        
        T ResolveByTag(TComposite composite, object tag);
    }
}
#endif