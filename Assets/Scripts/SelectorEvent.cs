//using Unity.VisualScripting;
//using UnityEngine;
//using static UnityEngine.UI.Button;

//public class EventNames
//{
//    public static string Selecting = "SelectorEvent";
//}

//[UnitTitle("On Selecting")]//Custom EventUnit to receive the event. Adding On to the unit title as an event naming convention.
//[UnitCategory("Events\\MyEvents")]//Setting the path to find the unit in the fuzzy finder in Events > My Events.

//public class SelectingEvent : EventUnit<ButtonClickedEvent>
//{
//    [DoNotSerialize]// No need to serialize ports.
//    public ValueOutput clickedEvent { get; private set; }// The event output data to return when the event is triggered.
//    protected override bool register => true;

//    // Adding an EventHook with the name of the event to the list of visual scripting events.
//    public override EventHook GetHook(GraphReference reference)
//    {
//        return new EventHook(EventNames.Selecting);
//    }
//    protected override void Definition()
//    {
//        base.Definition();
//        // Setting the value on our port.
//        clickedEvent = ValueOutput<ButtonClickedEvent>(nameof(clickedEvent));
//    }
//    // Setting the value on our port.
//    protected override void AssignArguments(Flow flow, ButtonClickedEvent data)
//    {
//        flow.SetValue(clickedEvent, data);
//    }
//}

//[UnitTitle("Send Selecting Event")]
//[UnitCategory("Events\\MyEvents")]//Setting the path to find the unit in the fuzzy finder in Events > My Events.
//public class SendSelectingEvent: Unit
//{
//    [DoNotSerialize]// Mandatory attribute, to make sure we don’t serialize data that should never be serialized.
//    [PortLabelHidden]// Hiding the port label as we normally hide the label for default Input and Output triggers.
//    public ControlInput inputTrigger { get; private set; }
//    [DoNotSerialize]
//    public ValueInput gameObject;
//    [DoNotSerialize]
//    public ValueInput value;
//    [DoNotSerialize]
//    [PortLabelHidden]// Hiding the port label as we normally hide the label for default Input and Output triggers.
//    public ControlOutput outputTrigger { get; private set; }

//    protected override void Definition()
//    {

//        inputTrigger = ControlInput(nameof(inputTrigger), Trigger);
//        gameObject = ValueInput<GameObject>(nameof(gameObject), null);
//        value = ValueInput<int>(nameof(value), 0);
//        outputTrigger = ControlOutput(nameof(outputTrigger));
//        Succession(inputTrigger, outputTrigger);
//    }

//    //Sending the Event MyCustomEvent with the integer value from the ValueInput port myValueA.
//    private ControlOutput Trigger(Flow flow)
//    {
//        //EventBus.Trigger(EventNames.Selecting, flow.GetValue<int>(value));
//        EventBus.Trigger(EventNames.Selecting);
//        return outputTrigger;
//    }
//}
