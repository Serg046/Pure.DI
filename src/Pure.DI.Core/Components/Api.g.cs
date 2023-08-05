// <auto-generated/>
#if !PUREDI_API_SUPPRESSION || PUREDI_API_V2
#pragma warning disable

#if NET20 || NET35 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6
namespace System.Diagnostics.CodeAnalysis
{
    // ReSharper disable UnusedType.Global
    [global::System.AttributeUsage(global::System.AttributeTargets.Assembly | global::System.AttributeTargets.Class | global::System.AttributeTargets.Constructor | global::System.AttributeTargets.Event | global::System.AttributeTargets.Method | global::System.AttributeTargets.Property | global::System.AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public sealed class ExcludeFromCodeCoverageAttribute : global::System.Attribute
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
    using global::System;
    using global::System.Diagnostics;

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
        /// Lifetime per resolve is similar to <see cref="Lifetime.Transient"/>, but the instance is reused in the same object graph.
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
        /// <c>On</c> or <c>Off</c>. Determines whether to generate partial <c>OnNewInstance</c> method. <c>Off</c> by default.
        /// </summary>
        OnNewInstance,
        
        /// <summary>
        /// The regular expression to filter OnNewInstance by the instance type name. ".+" by default.
        /// </summary>
        OnNewInstanceImplementationTypeNameRegularExpression,
        
        /// <summary>
        /// The regular expression to filter OnNewInstance by the tag. ".+" by default.
        /// </summary>
        OnNewInstanceTagRegularExpression,
        
        /// <summary>
        /// The regular expression to filter OnNewInstance by the lifetime. ".+" by default.
        /// </summary>
        OnNewInstanceLifetimeRegularExpression,
        
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
        /// <c>On</c> or <c>Off</c>. Determines whether to generate a static partial <c>OnNewRoot<T>(...)</c> method to handle the new Composition root registration event. <c>Off</c> by default.
        /// </summary>
        OnNewRoot,
        
        /// <summary>
        /// <c>On</c> or <c>Off</c>. Determine if the <c>ToString()</c> method should be generated. This method provides a text-based class diagram in the format mermaid. <c>Off</c> by default. 
        /// </summary>
        ToString,
        
        /// <summary>
        /// <c>On</c> or <c>Off</c>. This hint determines whether object Composition will be created in a thread-safe manner. <c>On</c> by default. 
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
    /// Represents a generic argument token. It allows you to create custom generic argument tokens such as <see cref="TTS"/>, <see cref="TTDictionary{TKey,TValue}"/>, etc. 
    /// </summary>
    [global::System.AttributeUsage(global::System.AttributeTargets.Class | global::System.AttributeTargets.Interface | global::System.AttributeTargets.Struct)]
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal sealed class GenericTypeArgumentAttribute : global::System.Attribute { }
    
    /// <summary>
    /// Represents an ordinal attribute. For constructors, it defines the sequence of attempts to use a particular constructor to create an object. For fields, properties and methods, it specifies to perform dependency injection and defines the sequence.
    /// </summary>
    [global::System.AttributeUsage(global::System.AttributeTargets.Constructor | global::System.AttributeTargets.Method | global::System.AttributeTargets.Property | global::System.AttributeTargets.Field, AllowMultiple = false)]
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal class OrdinalAttribute : global::System.Attribute
    {
        /// <summary>
        /// Creates an attribute instance.
        /// </summary>
        /// <param name="ordinal">The injection ordinal.</param>
        public OrdinalAttribute(int ordinal) { }
    }

    /// <summary>
    /// Represents a tag attribute overriding an injection tag.
    /// </summary>
    [global::System.AttributeUsage(global::System.AttributeTargets.Parameter | global::System.AttributeTargets.Property | global::System.AttributeTargets.Field, AllowMultiple = false)]
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal class TagAttribute : global::System.Attribute
    {
        /// Creates an attribute instance.
        /// </summary>
        /// <param name="tag">The injection tag. See also <see cref="IBinding.Tags"/></param>.
        public TagAttribute(object tag) { }
    }

    /// <summary>
    /// Represents a dependency type attribute overriding an injection type. 
    /// </summary>
    [global::System.AttributeUsage(global::System.AttributeTargets.Parameter | global::System.AttributeTargets.Property | global::System.AttributeTargets.Field, AllowMultiple = false)]
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal class TypeAttribute : global::System.Attribute
    {
        /// <summary>
        /// Creates an attribute instance.
        /// </summary>
        /// <param name="type">The injection type. See also <see cref="IConfiguration.Bind{T}"/> and <see cref="IBinding.Bind{T}"/>.</param>
        public TypeAttribute(global::System.Type type) { }
    }

    /// <summary>
    /// Determines how the partial class will be generated.
    /// </summary>
    internal enum CompositionKind
    {
        /// <summary>
        /// This value is used by default. If this value is specified, a normal partial class will be generated.
        /// </summary>
        Public,
        
        /// <summary>
        /// If this value is specified, the class will not be generated, but this setting can be used by other users as a baseline. `DependsOn(...)` is mandatory
        /// </summary>
        Internal,
        
        /// <summary>
        /// No partial classes will be created when this value is specified, but this setting is the baseline for all installations in the current project, and `DependsOn(...)` is not required.
        /// </summary>
        Global
    }
    
    /// <summary>
    /// An API for a Dependency Injection setup.
    /// </summary>
    internal interface IConfiguration
    {
        /// <summary>
        /// Begins the definition of the binding.
        /// <example><code>Bind&lt;IService&gt;()</code></example>>
        /// </summary>
        /// <typeparam name="T">The type of dependency to be bound.</typeparam>
        /// <param name="tags">The optional argument that specifies tags for a particular type of dependency binding.</param>
        /// <returns>API reference to the installation continuation chain.</returns>
        IBinding Bind<T>(params object[] tags);

        /// <summary>
        /// Indicates the use of some single or multiple setups as base setups by name.
        /// <example><code><c>DependsOn(nameof(CompositionBase))</c></code></example>
        /// </summary>
        /// <param name="baseConfigurationNames">The name of a base API reference to the installation continuation chain.</param>
        /// <returns>API reference to the installation continuation chain.</returns>
        IConfiguration DependsOn(params string[] baseConfigurationNames);

        /// <summary>
        /// Specifies a custom attribute that overrides the injection type.
        /// </summary>
        /// <example><code>TypeAttribute&lt;MyTypeAttribute&gt;()</code></example>
        /// <param name="typeArgumentPosition">The optional parameter that specifies the position of the type parameter in the attribute constructor. 0 by default. See predefined attribute <see cref="TypeAttribute{T}"/>.</param>
        /// <typeparam name="T">The attribute type.</typeparam>
        /// <returns>API reference to the installation continuation chain.</returns>
        IConfiguration TypeAttribute<T>(int typeArgumentPosition = 0) where T : global::System.Attribute;

        /// <summary>
        /// Specifies a tag attribute that overrides the injected tag.
        /// </summary>
        /// <example><code>TagAttribute&lt;MyTagAttribute&gt;()</code></example>
        /// <param name="tagArgumentPosition">The optional parameter that specifies the position of the tag parameter in the attribute constructor. 0 by default. See the predefined <see cref="TagAttribute{T}"/> attribute.</param>
        /// <typeparam name="T">The attribute type.</typeparam>
        /// <returns>API reference to the installation continuation chain.</returns>
        IConfiguration TagAttribute<T>(int tagArgumentPosition = 0) where T : global::System.Attribute;

        /// <summary>
        /// Specifies a custom attribute that overrides the injection ordinal.
        /// </summary>
        /// <example><code>OrdinalAttribute&lt;MyOrdinalAttribute&gt;()</code></example>
        /// <param name="ordinalArgumentPosition">The optional parameter that specifies the position of the ordinal parameter in the attribute constructor. 0 by default. See the predefined <see cref="OrdinalAttribute{T}"/> attribute.</param>
        /// <typeparam name="T">The attribute type.</typeparam>
        /// <returns>API reference to the installation continuation chain.</returns>
        IConfiguration OrdinalAttribute<T>(int ordinalArgumentPosition = 0) where T : global::System.Attribute;

        /// <summary>
        /// Overrides the default <see cref="Lifetime"/> for all bindings further down the chain. If not specified, the <see cref="Lifetime.Transient"/> lifetime is used.
        /// </summary>
        /// <example><code>DefaultLifetime(Lifetime.Singleton)</code></example>
        /// <param name="lifetime">The default lifetime.</param>
        /// <returns>API reference to the installation continuation chain.</returns>
        IConfiguration DefaultLifetime(Pure.DI.Lifetime lifetime);
        
        /// <summary>
        /// Adds a partial class argument and replaces the default constructor by adding this argument as a parameter. It is only created if this argument is actually used. 
        /// </summary>
        /// <example><code>Arg&lt;int&gt;("id")</code></example>
        /// <param name="name">The argument name.</param>
        /// <param name="tags">The optional argument that specifies the tags for the argument.</param>
        /// <typeparam name="T">The argument type.</typeparam>
        /// <returns>API reference to the installation continuation chain.</returns>
        IConfiguration Arg<T>(string name, params object[] tags);
        
        /// <summary>
        /// Specifying the root of the Composition.
        /// </summary>
        /// <example><code>Root&lt;IService&gt;("Root")</code></example>
        /// <param name="name">Specifies the unique name of the root of the composition. If the value is empty, a private root will be created, which can be used when calling <c>Resolve</c> methods.</param>
        /// <param name="tag">Optional argument specifying the tag for the root of the Composition.</param>
        /// <typeparam name="T">The Composition root type.</typeparam>
        /// <returns>API reference to the installation continuation chain.</returns>
        IConfiguration Root<T>(string name = "", object tag = null);

        /// <summary>
        /// Defines a hint for fine-tuning code generation.
        /// </summary>
        /// <example><code>Hint(Resolve, "Off")</code></example>
        /// <param name="hint">The hint type.</param>
        /// <param name="value">The hint value.</param>
        /// <returns>API reference to the installation continuation chain.</returns>
        IConfiguration Hint(Hint hint, string value);
    }

    /// <summary>
    /// An API for a binding setup.
    /// </summary>
    internal interface IBinding
    {
        /// <summary>
        /// Begins the definition of the binding.
        /// </summary>
        /// <example><code>Bind&lt;IService&gt;()</code></example>>
        /// <typeparam name="T">The type of dependency to be bound. Common type markers such as <see cref="TT"/>, <see cref="TTList{T}"/> and others are also supported.</typeparam>
        /// <param name="tags">The optional argument that specifies tags for a particular type of dependency binding.</param>
        /// <returns>API reference to the installation continuation chain.</returns>
        IBinding Bind<T>(params object[] tags);

        /// <summary>
        /// Determines the <see cref="Lifetime"/> of a binding.
        /// </summary>
        /// <example><code>As(Lifetime.Singleton)</code></example>
        /// <param name="lifetime">The <see cref="Lifetime"/> of a binding</param>
        /// <returns>API reference to the installation continuation chain.</returns>
        IBinding As(Pure.DI.Lifetime lifetime);

        /// <summary>
        /// Defines the binding tags.
        /// </summary>
        /// <example><code>Tags("myTag", 123)</code></example>
        /// <param name="tags">The binding tags.</param>
        /// <returns>API reference to the installation continuation chain.</returns>
        IBinding Tags(params object[] tags);

        /// <summary>
        /// Completes the binding chain by specifying the implementation.
        /// </summary>
        /// <example><code>To&lt;Service&gt;()</code></example>
        /// <typeparam name="T">The implementation type. Also supports generic type markers such as <see cref="TT"/>, <see cref="TTList{T}"/>, and others.</typeparam>
        /// <returns>API reference to the installation continuation chain.</returns>
        IConfiguration To<T>();

        /// <summary>
        /// Completes the binding chain by specifying the implementation using a factory method. It allows you to manually create an instance, call the necessary methods, initialize properties, fields, etc.
        /// </summary>
        /// <example><code>
        /// To(_ =&gt;
        /// {
        ///   var service = new Service("My Service");
        ///   service.Initialize();
        ///   return service;
        /// })
        /// </code></example>
        /// <param name="factory">Lambda expression to manually create an instance.</param>
        /// <typeparam name="T">The implementation type.</typeparam>
        /// <returns>API reference to the installation continuation chain.</returns>
        IConfiguration To<T>(global::System.Func<IContext, T> factory);
    }

    /// <summary>
    /// Abstract injection context./>.
    /// </summary>
    internal interface IContext
    {
        /// <summary>
        /// The tag that was used to inject the current object in the object graph.
        /// </summary>
        object Tag { get; }
            
        /// <summary>
        /// Injects an instance of type <c>T</c>.
        /// <example><code>ctx.Inject&lt;IDependency&gt;(out var dependency);</code></example>
        /// </summary>
        void Inject<T>(out T value);

        /// <summary>
        /// Injects an instance of type <c>T</c> marked with a tag.
        /// </summary>
        /// <example><code>ctx.Inject&lt;IDependency&gt;("myTag", out var dependency);</code></example>
        void Inject<T>(object tag, out T value);
    }
    
    /// <summary>
    /// An API for a Dependency Injection setup.
    /// </summary>
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static class DI
    {
        /// <summary>
        /// Begins the definitions of the Dependency Injection setup chain.
        /// </summary>
        /// <example><code>
        /// DI.Setup("Composition")
        ///   .Bind&lt;IDependency&gt;().To&lt;Dependency&gt;()
        ///   .Bind&lt;IService&gt;().To&lt;Service&gt;()
        ///   .Root&lt;IService&gt;("Root");
        /// </code></example>
        /// <param name="compositionTypeName">This argument specifying the partial class name to generate.</param>
        /// <param name="kind">An optional argument specifying the kind of setup. Please <see cref="Pure.DI.CompositionKind"/> for details. It defaults to <c>Public</c>.</param>
        /// <returns>API reference to the installation continuation chain.</returns>
        [global::System.Runtime.CompilerServices.MethodImpl((global::System.Runtime.CompilerServices.MethodImplOptions)0x300)]
        internal static IConfiguration Setup(string compositionTypeName, CompositionKind kind = CompositionKind.Public)
        {
            return Configuration.Shared;
        }

        private sealed class Configuration : IConfiguration
        {
            public static readonly IConfiguration Shared = new Configuration();

            private Configuration() { }

            /// <inheritdoc />
            [global::System.Runtime.CompilerServices.MethodImpl((global::System.Runtime.CompilerServices.MethodImplOptions)0x300)]
            public IBinding Bind<T>(params object[] tags)
            {
                return Binding.Shared;
            }

            /// <inheritdoc />
            [global::System.Runtime.CompilerServices.MethodImpl((global::System.Runtime.CompilerServices.MethodImplOptions)0x300)]
            public IConfiguration DependsOn(params string[] baseConfigurationName)
            {
                return Configuration.Shared;
            }

            /// <inheritdoc />
            [global::System.Runtime.CompilerServices.MethodImpl((global::System.Runtime.CompilerServices.MethodImplOptions)0x300)]
            public IConfiguration TypeAttribute<T>(int typeArgumentPosition = 0) where T : global::System.Attribute
            {
                return Configuration.Shared;
            }

            /// <inheritdoc />
            [global::System.Runtime.CompilerServices.MethodImpl((global::System.Runtime.CompilerServices.MethodImplOptions)0x300)]
            public IConfiguration TagAttribute<T>(int tagArgumentPosition = 0) where T : global::System.Attribute
            {
                return Configuration.Shared;
            }

            /// <inheritdoc />
            [global::System.Runtime.CompilerServices.MethodImpl((global::System.Runtime.CompilerServices.MethodImplOptions)0x300)]
            public IConfiguration OrdinalAttribute<T>(int ordinalArgumentPosition = 0) where T : global::System.Attribute
            {
                return Configuration.Shared;
            }

            /// <inheritdoc />
            [global::System.Runtime.CompilerServices.MethodImpl((global::System.Runtime.CompilerServices.MethodImplOptions)0x300)]
            public IConfiguration DefaultLifetime(Pure.DI.Lifetime lifetime)
            {
                return Configuration.Shared;
            }
            
            /// <inheritdoc />
            [global::System.Runtime.CompilerServices.MethodImpl((global::System.Runtime.CompilerServices.MethodImplOptions)0x300)]
            public IConfiguration Arg<T>(string name, params object[] tags)
            {
                return Configuration.Shared;
            }

            /// <inheritdoc />
            [global::System.Runtime.CompilerServices.MethodImpl((global::System.Runtime.CompilerServices.MethodImplOptions)0x300)]
            public IConfiguration Root<T>(string name, object tag)
            {
                return Configuration.Shared;
            }

            /// <inheritdoc />
            [global::System.Runtime.CompilerServices.MethodImpl((global::System.Runtime.CompilerServices.MethodImplOptions)0x300)]
            public IConfiguration Hint(Hint hint, string value)
            {
                return Configuration.Shared;
            }
        }

        private sealed class Binding : IBinding
        {
            public static readonly IBinding Shared = new Binding();

            private Binding() { }

            /// <inheritdoc />
            [global::System.Runtime.CompilerServices.MethodImpl((global::System.Runtime.CompilerServices.MethodImplOptions)0x300)]
            public IBinding Bind<T>(params object[] tags)
            {
                return Shared;
            }

            /// <inheritdoc />
            [global::System.Runtime.CompilerServices.MethodImpl((global::System.Runtime.CompilerServices.MethodImplOptions)0x300)]
            public IBinding As(Pure.DI.Lifetime lifetime)
            {
                return Shared;
            }

            /// <inheritdoc />
            [global::System.Runtime.CompilerServices.MethodImpl((global::System.Runtime.CompilerServices.MethodImplOptions)0x300)]
            public IBinding Tags(params object[] tags)
            {
                return Shared;
            }

            /// <inheritdoc />
            [global::System.Runtime.CompilerServices.MethodImpl((global::System.Runtime.CompilerServices.MethodImplOptions)0x300)]
            public IConfiguration To<T>()
            {
                return Configuration.Shared;
            }

            /// <inheritdoc />
            [global::System.Runtime.CompilerServices.MethodImpl((global::System.Runtime.CompilerServices.MethodImplOptions)0x300)]
            public IConfiguration To<T>(global::System.Func<IContext, T> factory)
            {
                return Configuration.Shared;
            }
        }
    }
    
    /// <summary>
    /// For internal use.
    /// </summary>
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]
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
    
    /// <summary>
    /// For internal use. 
    /// </summary>
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
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
            int[] bucketSizes = new int[divisor];
            for (int i = 0; i < pairs.Length; i++)
            {
                int bucket = (int)(((uint)global::System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(pairs[i].Key)) % divisor);
                int size = bucketSizes[bucket] + 1;
                bucketSizes[bucket] = size;
                if (size > bucketSize)
                {
                    bucketSize = size;
                }
            }
            
            Pair<TKey, TValue>[] buckets = new Pair<TKey, TValue>[divisor * bucketSize];
            for (int i = 0; i < pairs.Length; i++)
            {
                int bucket = (int)(((uint)global::System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(pairs[i].Key)) % divisor);
                var index = bucketSizes[bucket];
                buckets[bucket * bucketSize + bucketSize - index] = pairs[i];
                bucketSizes[bucket] = index - 1;
            }
            
            return buckets;
        }
    }

    /// <summary>
    /// Abstract dependency resolver.
    /// </summary>
    /// <typeparam name="TComposite">The Composition instance.</typeparam>
    /// <typeparam name="T">The type of the Composition root.</typeparam>
    internal interface IResolver<TComposite, out T>
    {
        /// <summary>
        /// Resolves the Composition root.
        /// </summary>
        /// <param name="composite">The Composition instance.</param>
        /// <returns>The Compositional root.</returns>
        T Resolve(TComposite composite);
        
        /// <summary>
        /// Resolves the Composition root by type and tag.
        /// </summary>
        /// <param name="composite">The Composition instance.</param>
        /// <param name="tag">The tag of a Composition root.</param>
        /// <returns>Compositional root.</returns>
        T ResolveByTag(TComposite composite, object tag);
    }
}
#pragma warning restore
#endif