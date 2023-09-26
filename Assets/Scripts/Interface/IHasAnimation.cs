using System.Collections.Generic;

public interface IHasAnimation
{
    public List<AnimatorParamData> AnimationDatas { get; set; }
    public AnimationController AnimationController { get;}

    public void ChangeAnimation(string animatorParam, object value);

    public void InitializeAnimationController();
}
