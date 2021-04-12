namespace Howatworks.SubEtha.Bindings
{
    public class BoundButton
    {
        public string BindingName { get; }
        public BindingActivationType Activation { get; }

        public BoundButton(string bindingName, BindingActivationType activation = BindingActivationType.Press)
        {
            BindingName = bindingName;
            Activation = activation;
        }
    }
}
