using System;
using Libplanet.Action;
using Libplanet.Blockchain.Renderers;
using Libplanet.Blocks;
using Libplanet.Tests.Common.Action;
using Xunit;

namespace Libplanet.Tests.Blockchain.Renderers
{
    public class AnonymousRendererTest
    {
        private static IAction _action = new DumbAction();

        private static IAccountStateDelta _stateDelta =
            new AccountStateDeltaImpl(_ => null, (_, __) => default, default);

        private static IActionContext _actionContext =
            new ActionContext(default, default, default, _stateDelta, default);

        private static Exception _exception = new Exception();

        private static Block<DumbAction> _genesis =
            TestUtils.MineGenesis<DumbAction>(default(Address));

        private static Block<DumbAction> _blockA = TestUtils.MineNext(_genesis);

        private static Block<DumbAction> _blockB = TestUtils.MineNext(_genesis);

        [Fact]
        public void ActionRenderer()
        {
            (IAction, IActionContext, IAccountStateDelta)? record = null;
            var renderer = new AnonymousRenderer<DumbAction>
            {
                ActionRenderer = (action, context, nextStates) =>
                    record = (action, context, nextStates),
            };

            renderer.UnrenderAction(_action, _actionContext, _stateDelta);
            Assert.Null(record);
            renderer.RenderActionError(_action, _actionContext, _exception);
            Assert.Null(record);
            renderer.UnrenderActionError(_action, _actionContext, _exception);
            Assert.Null(record);
            renderer.RenderBlock(_genesis, _blockA);
            Assert.Null(record);
            renderer.RenderReorg(_blockA, _blockB, _genesis);
            Assert.Null(record);

            renderer.RenderAction(_action, _actionContext, _stateDelta);
            Assert.NotNull(record);
            Assert.Same(_action, record?.Item1);
            Assert.Same(_actionContext, record?.Item2);
            Assert.Same(_stateDelta, record?.Item3);
        }

        [Fact]
        public void ActionUnrenderer()
        {
            (IAction, IActionContext, IAccountStateDelta)? record = null;
            var renderer = new AnonymousRenderer<DumbAction>
            {
                ActionUnrenderer = (action, context, nextStates) =>
                    record = (action, context, nextStates),
            };

            renderer.RenderAction(_action, _actionContext, _stateDelta);
            Assert.Null(record);
            renderer.RenderActionError(_action, _actionContext, _exception);
            Assert.Null(record);
            renderer.UnrenderActionError(_action, _actionContext, _exception);
            Assert.Null(record);
            renderer.RenderBlock(_genesis, _blockA);
            Assert.Null(record);
            renderer.RenderReorg(_blockA, _blockB, _genesis);
            Assert.Null(record);

            renderer.UnrenderAction(_action, _actionContext, _stateDelta);
            Assert.NotNull(record);
            Assert.Same(_action, record?.Item1);
            Assert.Same(_actionContext, record?.Item2);
            Assert.Same(_stateDelta, record?.Item3);
        }

        [Fact]
        public void ActionErrorRenderer()
        {
            (IAction, IActionContext, Exception)? record = null;
            var renderer = new AnonymousRenderer<DumbAction>
            {
                ActionErrorRenderer = (action, context, exception) =>
                    record = (action, context, exception),
            };

            renderer.RenderAction(_action, _actionContext, _stateDelta);
            Assert.Null(record);
            renderer.UnrenderAction(_action, _actionContext, _stateDelta);
            Assert.Null(record);
            renderer.UnrenderActionError(_action, _actionContext, _exception);
            Assert.Null(record);
            renderer.RenderBlock(_genesis, _blockA);
            Assert.Null(record);
            renderer.RenderReorg(_blockA, _blockB, _genesis);
            Assert.Null(record);

            renderer.RenderActionError(_action, _actionContext, _exception);
            Assert.NotNull(record);
            Assert.Same(_action, record?.Item1);
            Assert.Same(_actionContext, record?.Item2);
            Assert.Same(_exception, record?.Item3);
        }

        [Fact]
        public void ActionErrorUnrenderer()
        {
            (IAction, IActionContext, Exception)? record = null;
            var renderer = new AnonymousRenderer<DumbAction>
            {
                ActionErrorUnrenderer = (action, context, exception) =>
                    record = (action, context, exception),
            };

            renderer.RenderAction(_action, _actionContext, _stateDelta);
            Assert.Null(record);
            renderer.UnrenderAction(_action, _actionContext, _stateDelta);
            Assert.Null(record);
            renderer.RenderActionError(_action, _actionContext, _exception);
            Assert.Null(record);
            renderer.RenderBlock(_genesis, _blockA);
            Assert.Null(record);
            renderer.RenderReorg(_blockA, _blockB, _genesis);
            Assert.Null(record);

            renderer.UnrenderActionError(_action, _actionContext, _exception);
            Assert.NotNull(record);
            Assert.Same(_action, record?.Item1);
            Assert.Same(_actionContext, record?.Item2);
            Assert.Same(_exception, record?.Item3);
        }

        [Fact]
        public void BlockRenderer()
        {
            (Block<DumbAction> Old, Block<DumbAction> New)? record = null;
            var renderer = new AnonymousRenderer<DumbAction>
            {
                BlockRenderer = (oldTip, newTip) => record = (oldTip, newTip),
            };

            renderer.RenderAction(_action, _actionContext, _stateDelta);
            Assert.Null(record);
            renderer.UnrenderAction(_action, _actionContext, _stateDelta);
            Assert.Null(record);
            renderer.RenderActionError(_action, _actionContext, _exception);
            Assert.Null(record);
            renderer.UnrenderActionError(_action, _actionContext, _exception);
            Assert.Null(record);
            renderer.RenderReorg(_blockA, _blockB, _genesis);
            Assert.Null(record);

            renderer.RenderBlock(_genesis, _blockA);
            Assert.NotNull(record);
            Assert.Same(_genesis, record?.Old);
            Assert.Same(_blockA, record?.New);
        }

        [Fact]
        public void BlockReorg()
        {
            (Block<DumbAction> Old, Block<DumbAction> New, Block<DumbAction> Bp)? record = null;
            var renderer = new AnonymousRenderer<DumbAction>
            {
                ReorgRenderer = (oldTip, newTip, bp) => record = (oldTip, newTip, bp),
            };

            renderer.RenderAction(_action, _actionContext, _stateDelta);
            Assert.Null(record);
            renderer.UnrenderAction(_action, _actionContext, _stateDelta);
            Assert.Null(record);
            renderer.RenderActionError(_action, _actionContext, _exception);
            Assert.Null(record);
            renderer.UnrenderActionError(_action, _actionContext, _exception);
            Assert.Null(record);
            renderer.RenderBlock(_genesis, _blockA);
            Assert.Null(record);

            renderer.RenderReorg(_blockA, _blockB, _genesis);
            Assert.NotNull(record);
            Assert.Same(_blockA, record?.Old);
            Assert.Same(_blockB, record?.New);
            Assert.Same(_genesis, record?.Bp);
        }
    }
}
