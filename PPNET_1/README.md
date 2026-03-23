## Struktura Projektu

1. PPNET_1 -> Aplikacja konsolowa rozwiązująca problem plecakowy.
2. PPNET_1.Tests -> Testy jednostkowe dla klasy Problem.
3. PPNET_1.GUI -> Aplikacja graficzna do rozwiązania problemu plecakowego.

## Działanie algorytmu rozwiązującego problem plecakowy 

1. Oblicza stosunek wartości do wagi dla każdego przedmiotu (ratio = value/weight).
2. Sortuje przedmioty według tego stosunku w kolejności malejącej.
3. Wybiera przedmioty, zaczynając od tego o najwyższym stosunku, aż do momentu gdy nie można już dodać kolejnego przedmiotu.

## Testy jednostkowe

Test 1: Sprawdza, czy wygenerowanie problemu z n elementami powoduje utworzenie dokładnie n elementów.
Test 2: Sprawdza, czy jeśli przynajmniej jeden przedmiot spełnia ograniczenia to zwrócono co najmniej jeden element.
Test 3: Sprawdza, czy jeśli żaden przedmiot nie spełnia ograniczenia to zwrócono puste rozwiązanie.
Test 4: Weryfikuje poprawność dla konkretnej instancji.
Test 5: Sprawdza, czy algorytm greedy respektuje ograniczenia pojemności.
Test 6: Sprawdza, czy elementy mają prawidłowe właściwości.

## Główne klasy 

"Item" -> Reprezentuje pojedynczy przedmiot z wartością, wagą i indeksem.

"Problem" -> Reprezentuje instancję problemu (generuje 'n' losowych przedmiotów i rozwiązuje problem algorytmem zachłannym).

"Result" -> Przechowuje rozwiązanie (Lista  wybranych przedmiotów, łączna wartość, łączna waga).