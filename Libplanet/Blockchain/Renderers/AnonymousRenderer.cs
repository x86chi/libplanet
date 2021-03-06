#nullable enable
using System;
using Libplanet.Action;
using Libplanet.Blocks;

namespace Libplanet.Blockchain.Renderers
{
    /// <summary>
    /// A renderer that invokes its callbacks.
    /// <para>This class is useful when you want an one-use ad-hoc implementation (i.e., Java-style
    /// anonymous class) of <see cref="IRenderer{T}"/> interface.</para>
    /// </summary>
    /// <example>
    /// With object initializers, you can easily make an one-use renderer:
    /// <code>
    /// var renderer = new AnonymousRenderer&lt;ExampleAction&gt;
    /// {
    ///     ActionRenderer = (action, context, nextStates) =>
    ///     {
    ///         // Implement RenderAction() here.
    ///     };
    /// };
    /// </code>
    /// </example>
    /// <typeparam name="T">An <see cref="IAction"/> type.  It should match to
    /// <see cref="BlockChain{T}"/>'s type parameter.</typeparam>
    public sealed class AnonymousRenderer<T> : IRenderer<T>
        where T : IAction, new()
    {
        /// <summary>
        /// A callback function to be invoked together with
        /// <see cref="RenderAction(IAction, IActionContext, IAccountStateDelta)"/>.
        /// </summary>
        public Action<IAction, IActionContext, IAccountStateDelta>? ActionRenderer { get; set; }

        /// <summary>
        /// A callback function to be invoked together with
        /// <see cref="UnrenderAction(IAction, IActionContext, IAccountStateDelta)"/>.
        /// </summary>
        public Action<IAction, IActionContext, IAccountStateDelta>? ActionUnrenderer { get; set; }

        /// <summary>
        /// A callback function to be invoked together with
        /// <see cref="RenderActionError(IAction, IActionContext, Exception)"/>.
        /// </summary>
        public Action<IAction, IActionContext, Exception>? ActionErrorRenderer { get; set; }

        /// <summary>
        /// A callback function to be invoked together with
        /// <see cref="UnrenderActionError(IAction, IActionContext, Exception)"/>.
        /// </summary>
        public Action<IAction, IActionContext, Exception>? ActionErrorUnrenderer { get; set; }

        /// <summary>
        /// A callback function to be invoked together with
        /// <see cref="RenderBlock(Block{T}, Block{T})"/>.
        /// </summary>
        public Action<Block<T>, Block<T>>? BlockRenderer { get; set; }

        /// <summary>
        /// A callback function to be invoked together with
        /// <see cref="RenderReorg(Block{T}, Block{T}, Block{T})"/>.
        /// </summary>
        public Action<Block<T>, Block<T>, Block<T>>? ReorgRenderer { get; set; }

        /// <inheritdoc
        /// cref="IRenderer{T}.RenderAction(IAction, IActionContext, IAccountStateDelta)"/>
        public void RenderAction(
            IAction action,
            IActionContext context,
            IAccountStateDelta nextStates
        ) =>
            ActionRenderer?.Invoke(action, context, nextStates);

        /// <inheritdoc
        /// cref="IRenderer{T}.UnrenderAction(IAction, IActionContext, IAccountStateDelta)"/>
        public void UnrenderAction(
            IAction action,
            IActionContext context,
            IAccountStateDelta nextStates
        ) =>
            ActionUnrenderer?.Invoke(action, context, nextStates);

        /// <inheritdoc cref="IRenderer{T}.RenderActionError(IAction, IActionContext, Exception)"/>
        public void RenderActionError(IAction action, IActionContext context, Exception exception)
            => ActionErrorRenderer?.Invoke(action, context, exception);

        /// <inheritdoc
        /// cref="IRenderer{T}.UnrenderActionError(IAction, IActionContext, Exception)"/>
        public void UnrenderActionError(IAction action, IActionContext context, Exception exception)
            => ActionErrorUnrenderer?.Invoke(action, context, exception);

        /// <inheritdoc cref="IRenderer{T}.RenderBlock(Block{T}, Block{T})"/>
        public void RenderBlock(Block<T> oldTip, Block<T> newTip) =>
            BlockRenderer?.Invoke(oldTip, newTip);

        /// <inheritdoc cref="IRenderer{T}.RenderReorg(Block{T}, Block{T}, Block{T})"/>
        public void RenderReorg(Block<T> oldTip, Block<T> newTip, Block<T> branchpoint) =>
            ReorgRenderer?.Invoke(oldTip, newTip, branchpoint);
    }
}
