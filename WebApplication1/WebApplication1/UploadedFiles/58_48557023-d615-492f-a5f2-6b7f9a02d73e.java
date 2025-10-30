public class Person {
    int id;
    int currentFloor;
    int destinationFloor;
    int weight;
    boolean inElevator;

    public Person(int id, int currentFloor, int destinationFloor, int weight) {
        this.id = id;
        this.currentFloor = currentFloor;
        this.destinationFloor = destinationFloor;
        this.weight = weight;
        this.inElevator = false;
    }

    public boolean isGoingUp() {
        return destinationFloor > currentFloor;
    }
}
